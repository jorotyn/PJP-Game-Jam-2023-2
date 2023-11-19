using MoreMountains.Tools;
using UnityEngine;

public class ResourceManagementSystem : MMSingleton<ResourceManagementSystem>
{
    #region Serialized Fields
    [SerializeField] private Light directionalLight;
    [SerializeField] private float sunriseAngle = 0f;
    [SerializeField] private float zenithAngle = 90f;
    [SerializeField] private float sunsetAngle = 180f;
    [SerializeField] private float maxLightIntensity = 1f;
    [SerializeField] private float minLightIntensity = 0.3f;
    [SerializeField] private float intensityTransitionSpeed = 0.1f;

    [SerializeField] private float waterConsumptionRate = 1f;
    [SerializeField] private float nutrientConsumptionRate = 1f;
    [SerializeField] private float maxResourceLevel = 100f;
    [SerializeField] private float rainWaterIncreaseRate = 2f;
    [SerializeField] private float overcastSunlightReduction = 0.5f;
    [SerializeField] private float initialWaterLevel = 50f;
    [SerializeField] private float initialNutrientLevel = 50f;
    #endregion

    #region Private Fields
    private float sunlightLevel;
    private float waterLevel;
    private float nutrientLevel;
    private bool isRaining;
    private bool isOvercast;
    private int countOfSunlightAmplifiers;
    private float targetIntensity;
    #endregion

    #region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Update()
    {
        GetWeatherType();
        UpdateResources();
    }
    #endregion

    #region Initialization
    private void Initialize()
    {
        waterLevel = initialWaterLevel;
        nutrientLevel = initialNutrientLevel;
    }
    #endregion

    #region Private Methods
    private void GetWeatherType()
    {
        isOvercast = WeatherSystem.Instance.GetIsOvercast();
        isRaining = WeatherSystem.Instance.GetIsRaining();
    }

    private void UpdateResources()
    {
        UpdateSunlight();
        UpdateWaterLevel();
        ConsumeResources();
    }

    private void UpdateSunlight()
    {
        // Calculate base sunlight level
        float newSunlightLevel = Mathf.PingPong(Time.time, maxResourceLevel);

        // Determine AM or PM phase
        bool isAM = newSunlightLevel >= sunlightLevel; // AM if the new level is higher or equal to the previous
        sunlightLevel = newSunlightLevel;

        // Adjust sunlight level for overcast
        float adjustedSunlightLevel = isOvercast ? sunlightLevel * (1 - overcastSunlightReduction) : sunlightLevel;

        // Calculate sun rotation based on sunlight level
        float sunRotation;
        if (isAM)
        {
            // From sunrise to zenith
            sunRotation = Mathf.Lerp(sunriseAngle, zenithAngle, sunlightLevel / maxResourceLevel);
        }
        else
        {
            // From zenith to sunset
            sunRotation = Mathf.Lerp(zenithAngle, sunsetAngle, 1 - (sunlightLevel / maxResourceLevel));
        }

        // Calculate target light intensity based on overcast
        targetIntensity = isOvercast ? Mathf.Lerp(minLightIntensity, maxLightIntensity, 1 - overcastSunlightReduction) : maxLightIntensity;

        // Smoothly interpolate the light's intensity towards the target
        if (directionalLight != null)
        {
            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, targetIntensity, intensityTransitionSpeed * Time.deltaTime);
            directionalLight.transform.rotation = Quaternion.Euler(sunRotation, 0, 0);
        }
    }


    private void UpdateWaterLevel()
    {
        if (isRaining)
        {
            waterLevel = Mathf.Clamp(waterLevel + (rainWaterIncreaseRate * Time.deltaTime), 0f, maxResourceLevel);
        }
    }


    private void ConsumeResources()
    {
        waterLevel = Mathf.Clamp(waterLevel - (waterConsumptionRate * Time.deltaTime), 0f, maxResourceLevel);
        nutrientLevel = Mathf.Clamp(nutrientLevel - (nutrientConsumptionRate * Time.deltaTime), 0f, maxResourceLevel);
    }
    #endregion

    #region Public Methods
    public void AddWater(float amount)
    {
        waterLevel = Mathf.Clamp(waterLevel + amount, 0f, maxResourceLevel);
    }

    public void AddNutrients(float amount)
    {
        nutrientLevel = Mathf.Clamp(nutrientLevel + amount, 0f, maxResourceLevel);
    }

    public void SetRaining(bool raining)
    {
        isRaining = raining;
    }

    public void SetOvercast(bool overcast)
    {
        isOvercast = overcast;
    }

    public float GetSunlightLevel()
    {
        return sunlightLevel;
    }

    public float GetWaterLevel()
    {
        return waterLevel;
    }

    public float GetNutrientLevel()
    {
        return nutrientLevel;
    }

    public int GetNumberOfSunlightAmplifiers()
    {
        return countOfSunlightAmplifiers;
    }

    public void IncrementSunlightAmplifiers()
    {
        countOfSunlightAmplifiers++;
    }

    public void DecrementSunlightAmplifiers()
    {
        countOfSunlightAmplifiers--;
    }
    #endregion
}
