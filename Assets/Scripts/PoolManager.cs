
// Put poolable GameObject inside "Resources/PoolableGameObjects"

using MoreMountains.Tools;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MMSingleton<PoolManager>
{
	public GameObject Get<T>() where T :  IPoolableGameObject
	{
		GameObject go = null;
		System.Type type = typeof(T);
		if (!_pools[type].pooledGameObjects.TryTake(out go))
			go = Instantiate(_pools[type].prefab);

		var poolableObject = go.GetComponents(typeof(Component)).Where(component => component is IPoolableGameObject).First();
		((IPoolableGameObject)poolableObject).WakeUp();
		return go;
	}

	public void Return<T>(GameObject go) where T : IPoolableGameObject
	{
		var poolableObject = go.GetComponents(typeof(Component)).Where(component => component is IPoolableGameObject).First();
		((IPoolableGameObject)poolableObject).Sleep();
		_pools[typeof(T)].pooledGameObjects.Add(go);
	}


	private class Pool
	{
		public GameObject prefab;
		public ConcurrentBag<GameObject> pooledGameObjects;
	}

	private Dictionary<System.Type, Pool> _pools;

	protected override void Awake()
	{
		base.Awake();
		Initialize();
	}

	private void Initialize()
	{
		_pools = new Dictionary<System.Type, Pool>();
		Object[] objs = Resources.LoadAll("PoolableGameObjects");
		foreach (var obj in objs)
		{
			string name = obj.name;

			if (obj is not GameObject)
			{
				Debug.LogError($"Object {name} is not a GameObject");
				continue;
			}

			#if UNITY_EDITOR
			if (UnityEditor.PrefabUtility.GetPrefabAssetType(obj) == UnityEditor.PrefabAssetType.NotAPrefab)
			{
				Debug.LogError($"GameObject {name} is not a prefab");
				continue;
			}
			#endif

			GameObject go = (GameObject)obj;
			List<Component> components = go.GetComponents(typeof(Component)).Where(component => component is IPoolableGameObject).ToList();
			if (components.Count == 0)
			{
				Debug.LogError($"Prefab {name} does not implement IPoolableGo");
				continue;
			}

			if (components.Count > 1)
			{
				Debug.LogError($"Prefab {name} implements IPoolableGo multiple times");
				continue;
			}

			_pools.Add(components[0].GetType(), new Pool() { prefab = go, pooledGameObjects = new ConcurrentBag<GameObject>() });
		}
	}
}
