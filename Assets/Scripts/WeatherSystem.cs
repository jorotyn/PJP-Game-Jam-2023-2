using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Events;

public class WeatherSystem : MMSingleton<WeatherSystem>
{
    public enum WeatherType { Sunny, Overcast, Raining }

    #region Serialized Fields
    [Header("References")]
    [SerializeField] private ParticleSystem rainParticleSystem;

    [Header("Settings")]
    [SerializeField] private float minWeatherDuration = 10f;
    [SerializeField] private float maxWeatherDuration = 30f;

    [SerializeField] private UnityEvent onSunnyWeatherStart;
    [SerializeField] private UnityEvent onOvercastWeatherStart;
    [SerializeField] private UnityEvent onRainingWeatherStart;
    #endregion

    #region Private Fields
    private WeatherType currentWeather;
    private float weatherTimer;
    private float nextWeatherChange;
    private bool isOvercast;
    private bool isRaining;
    private bool weatherChanged;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeWeather();
    }

    private void Update()
    {
        UpdateWeather();
        ApplyWeatherEffects();
    }
    #endregion

    #region Initialization
    private void InitializeWeather()
    {
        SetRandomWeather();
        SetNextWeatherChange();
        StopRainParticleSystem();
        weatherChanged = true;
    }
    #endregion

    #region Private Methods
    private void UpdateWeather()
    {
        weatherTimer += Time.deltaTime;

        if (weatherTimer >= nextWeatherChange)
        {
            SetRandomWeather();
            SetNextWeatherChange();
        }
    }

    private void ApplyWeatherEffects()
    {
        if (!weatherChanged) return;

        switch (currentWeather)
        {
            case WeatherType.Sunny:
                isRaining = false;
                isOvercast = false;
                onSunnyWeatherStart.Invoke();
                StopRainParticleSystem();
                break;
            case WeatherType.Overcast:
                isRaining = false;
                isOvercast = true;
                onOvercastWeatherStart.Invoke();
                StopRainParticleSystem();
                break;
            case WeatherType.Raining:
                isRaining = true;
                isOvercast = true;
                onRainingWeatherStart.Invoke();
                StartRainParticleSystem();
                break;
        }

        weatherChanged = false;
    }

    private void SetRandomWeather()
    {
        WeatherType newWeather;

        do
        {
            newWeather = (WeatherType)Random.Range(0, System.Enum.GetValues(typeof(WeatherType)).Length);
        } while (newWeather == currentWeather);

        currentWeather = newWeather;
        weatherChanged = true;
    }

    private void SetNextWeatherChange()
    {
        weatherTimer = 0f;
        nextWeatherChange = Random.Range(minWeatherDuration, maxWeatherDuration);
    }

    private void StartRainParticleSystem()
    {
        if (rainParticleSystem != null && !rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Play();
        }
    }

    private void StopRainParticleSystem()
    {
        if (rainParticleSystem != null && rainParticleSystem.isPlaying)
        {
            rainParticleSystem.Stop();
        }
    }
    #endregion

    #region Public Methods
    public WeatherType GetCurrentWeather()
    {
        return currentWeather;
    }

    public float GetWeatherTimeRemaining()
    {
        return nextWeatherChange - weatherTimer;
    }

    public bool GetIsOvercast()
    {
        return isOvercast;
    }

    public bool GetIsRaining()
    {
        return isRaining;
    }
    #endregion
}
