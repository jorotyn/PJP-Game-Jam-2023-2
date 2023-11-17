using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MMSingleton<AudioManager>
{
	public float MasterVolume
	{
		get
		{
			return _masterVolume;
		}
		set
		{
			_masterVolume = value;
			foreach (AudioObject audioObject in _audioObjects)
				audioObject.GetComponent<AudioSource>().volume = value;
		}
	}

	public float MusicVolume
	{
		get
		{
			return _musicVolume;
		}
		set
		{
			_musicVolume = value;
			if (_musicObject != null)
				_musicObject.GetComponent<AudioSource>().volume = value * _masterVolume;
		}
	}


	public void PlayOnce(AudioClip audioClip)
	{
		GameObject audioObject = PoolManager.Instance.Get<AudioObject>();
		audioObject.GetComponent<AudioSource>().PlayOneShot(audioClip, MasterVolume);
		_audioObjects.Add(audioObject.GetComponent<AudioObject>());
	}

	[SerializeField][Range(0f, 1f)] private float _masterVolume = 0.25f;
	[SerializeField][Range(0f, 1f)] private float _musicVolume = 1.0f;
	[SerializeField] private AudioClip _musicClip;
	private List<AudioObject> _audioObjects = new List<AudioObject>();
	private GameObject _musicObject;

	private void Start()
	{
		if (_musicObject == null && _musicClip != null)
		{
			_musicObject = new GameObject();
			_musicObject.name = "GameMusic";
			_musicObject.AddComponent<AudioSource>();
			AudioSource musicSource = _musicObject.GetComponent<AudioSource>();
			musicSource.clip = _musicClip;
			musicSource.loop = true;
			musicSource.volume = _masterVolume * _musicVolume;
			musicSource.Play();
		}
	}

	private void Update()
	{
		#if UNITY_EDITOR
		MasterVolume = _masterVolume;
		MusicVolume = _musicVolume;
		#endif

		for (int i = _audioObjects.Count - 1; i >= 0; i--)
		{
			AudioSource audioSource = _audioObjects[i].GetComponent<AudioSource>();
			if (!audioSource.isPlaying)
			{
				PoolManager.Instance.Return<AudioObject>(_audioObjects[i].gameObject);
				_audioObjects.RemoveAt(i);
				continue;
			}
		}
	}
}
