using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(AudioSource))]
public class AudioObject : MonoBehaviour, IPoolableGameObject
{
	public void Sleep()
	{
		GetComponent<AudioSource>().Stop();
		gameObject.SetActive(false);
	}

	public void WakeUp()
	{
		gameObject.SetActive(true);
	}
}
