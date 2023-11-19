using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MMSingleton<AudioManager>
{
    [SerializeField][Range(0f, 1f)] private float _masterVolume = 0.25f;
    [SerializeField][Range(0f, 1f)] private float _musicVolume = 1.0f;
    [SerializeField] private List<AudioSource> musicTracks = new List<AudioSource>();
    [SerializeField] private bool playMusicOnAwake = true;
    [SerializeField] private float pauseBetweenTracks = 5f;
    [SerializeField] private AudioSource waterDepositAudioSource;
    [SerializeField] private AudioSource waterCollectAudioSource;
    [SerializeField] private AudioSource nutrientDepositAudioSource;
    [SerializeField] private AudioSource nutrientCollectAudioSource;
    [SerializeField] private AudioSource sunlightDepositAudioSource;
    [SerializeField] private AudioSource sunlightCollectAudioSource;
    [SerializeField] private AudioSource weedCutAudioSource;
    [SerializeField] private AudioSource weedSpawnAudioSource;
    private List<AudioObject> _audioObjects = new List<AudioObject>();
    private GameObject _musicObject;

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

    public void SetSettings(GameSettingsWrapper settings)
    {
        MasterVolume = settings.MasterVolume;
        MusicVolume = settings.MusicVolume;
    }

    public void GetSettings(GameSettingsWrapper settings)
    {
        settings.MasterVolume = MasterVolume;
        settings.MusicVolume = MusicVolume;
    }

    public void PlayOnce(AudioClip audioClip)
    {
        GameObject audioObject = PoolManager.Instance.Get<AudioObject>();
        audioObject.GetComponent<AudioSource>().PlayOneShot(audioClip, MasterVolume);
        _audioObjects.Add(audioObject.GetComponent<AudioObject>());
    }

    public void PlayWaterDeposit()
    {
        waterDepositAudioSource.Play();
    }

    public void PlayWaterCollect()
    {
        waterCollectAudioSource.Play();
    }

    public void PlayNutrientDeposit()
    {
        nutrientDepositAudioSource.Play();
    }

    public void PlayNutrientCollect()
    {
        nutrientCollectAudioSource.Play();
    }

    public void PlaySunlightDeposit()
    {
        sunlightDepositAudioSource.Play();
    }

    public void PlaySunlightCollect()
    {
        sunlightCollectAudioSource.Play();
    }

    public void PlayWeedCut()
    {
        weedCutAudioSource.Play();
    }

    public void PlayWeedSpawn()
    {
        weedSpawnAudioSource.Play();
    }

    private void Start()
    {
        if (playMusicOnAwake)
        {
            StartCoroutine(PlayMusicTracks());
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

    private IEnumerator PlayMusicTracks()
    {
        int currentTrackIndex = 0;

        while (true)
        {
            if (musicTracks.Count == 0)
                yield break;

            AudioSource currentTrack = musicTracks[currentTrackIndex];
            currentTrack.volume = _masterVolume * _musicVolume;
            currentTrack.Play();

            yield return new WaitForSeconds(currentTrack.clip.length + pauseBetweenTracks);

            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Count;
        }
    }
}
