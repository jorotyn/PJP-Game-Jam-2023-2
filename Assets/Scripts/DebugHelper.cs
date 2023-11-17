using TMPro;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private PlantGrowthSystem plantGrowthSystem;
    [SerializeField] private PlantHealthSystem plantHealthSystem;

    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI weatherTypeText;
    [SerializeField] private TextMeshProUGUI weatherTimeRemainingText;
    [SerializeField] private TextMeshProUGUI sunlightLevelText;
    [SerializeField] private TextMeshProUGUI waterLevelText;
    [SerializeField] private TextMeshProUGUI nutrientLevelText;
    [SerializeField] private TextMeshProUGUI plantGrowthText;
    [SerializeField] private TextMeshProUGUI plantStageText;
    [SerializeField] private TextMeshProUGUI plantHealthText;
    [SerializeField] private TextMeshProUGUI numberOfWeedsText;
    [SerializeField] private TextMeshProUGUI numberOfWorkersText;
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        WeatherSystem.WeatherType currentWeatherType = WeatherSystem.Instance.GetCurrentWeather();
        UpdateWeatherType(currentWeatherType);
        UpdateWeatherTimeRemaining(WeatherSystem.Instance.GetWeatherTimeRemaining());
        UpdateSunlightLevel(ResourceManagementSystem.Instance.GetSunlightLevel());
        UpdateWaterLevel(ResourceManagementSystem.Instance.GetWaterLevel());
        UpdateNutrientLevel(ResourceManagementSystem.Instance.GetNutrientLevel());
        UpdatePlantGrowth(plantGrowthSystem.GetCurrentGrowth());
        UpdatePlantStage(plantGrowthSystem.GetCurrentStage().ToString());
        UpdatePlantHealth(plantHealthSystem.GetHealth());
        UpdateNumberOfWeeds(WeedManager.Instance.GetTotalWeeds());
        //UpdateNumberOfWorkers();
    }
    #endregion

    #region Private Methods
    private void UpdateWeatherType(WeatherSystem.WeatherType weatherType)
    {
        if (weatherTypeText != null)
        {
            weatherTypeText.text = "Weather Type: " + weatherType.ToString();
        }
    }

    private void UpdateWeatherTimeRemaining(float timeRemaining)
    {
        if (weatherTimeRemainingText != null)
        {
            weatherTimeRemainingText.text = "Time Remaining: " + timeRemaining.ToString("F1") + "s";
        }
    }

    private void UpdateSunlightLevel(float sunlightLevel)
    {
        if (sunlightLevelText != null)
        {
            sunlightLevelText.text = "Sunlight Level: " + sunlightLevel.ToString("F1");
        }
    }

    private void UpdateWaterLevel(float waterLevel)
    {
        if (waterLevelText != null)
        {
            waterLevelText.text = "Water Level: " + waterLevel.ToString("F1");
        }
    }

    private void UpdateNutrientLevel(float nutrientLevel)
    {
        if (nutrientLevelText != null)
        {
            nutrientLevelText.text = "Nutrient Level: " + nutrientLevel.ToString("F1");
        }
    }

    private void UpdatePlantGrowth(float plantGrowth)
    {
        if (plantGrowthText != null)
        {
            plantGrowthText.text = "Plant Growth: " + plantGrowth.ToString("F1");
        }
    }

    private void UpdatePlantStage(string plantStage)
    {
        if (plantStageText != null)
        {
            plantStageText.text = "Plant Stage: " + plantStage;
        }
    }

    private void UpdatePlantHealth(float plantHealth)
    {
        if (plantHealthText != null)
        {
            plantHealthText.text = "Plant Health: " + plantHealth.ToString("F1");
        }
    }

    private void UpdateNumberOfWeeds(int numberOfWeeds)
    {
        if (numberOfWeedsText != null)
        {
            numberOfWeedsText.text = "Number of Weeds: " + numberOfWeeds;
        }
    }

    private void UpdateNumberOfWorkers(int numberOfWorkers)
    {
        if (numberOfWorkersText != null)
        {
            numberOfWorkersText.text = "Number of Workers: " + numberOfWorkers;
        }
    }
    #endregion
}
