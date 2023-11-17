using UnityEngine;
using UnityEngine.Events;

public class WeatherSystem : MonoBehaviour
{
    public enum WeatherType { Sunny, Overcast, Raining }

    #region Serialized Fields
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
        switch (currentWeather)
        {
            case WeatherType.Sunny:
                ResourceManagementSystem.Instance.SetRaining(false);
                ResourceManagementSystem.Instance.SetOvercast(false);
                onSunnyWeatherStart.Invoke();
                break;
            case WeatherType.Overcast:
                ResourceManagementSystem.Instance.SetRaining(false);
                ResourceManagementSystem.Instance.SetOvercast(true);
                onOvercastWeatherStart.Invoke();
                break;
            case WeatherType.Raining:
                ResourceManagementSystem.Instance.SetRaining(true);
                ResourceManagementSystem.Instance.SetOvercast(true);
                onRainingWeatherStart.Invoke();
                break;
        }
    }

    private void SetRandomWeather()
    {
        WeatherType newWeather;

        do
        {
            newWeather = (WeatherType)Random.Range(0, System.Enum.GetValues(typeof(WeatherType)).Length);
        } while (newWeather == currentWeather);

        currentWeather = newWeather;
    }

    private void SetNextWeatherChange()
    {
        weatherTimer = 0f;
        nextWeatherChange = Random.Range(minWeatherDuration, maxWeatherDuration);
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
    #endregion
}
