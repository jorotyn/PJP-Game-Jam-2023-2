using TMPro;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private PlantGrowthSystem plantGrowthSystem;
    [SerializeField] private PlantHealthSystem plantHealthSystem;

    [Header("Debug Elements")]
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

    [Header("Worker Elements")]
    [SerializeField] private TextMeshProUGUI totalWorkersText;
    [SerializeField] private TextMeshProUGUI idleWorkersText;
    [SerializeField] private TextMeshProUGUI workersClearingWeedsText;
    [SerializeField] private TextMeshProUGUI workersCollectingNutrientsText;
    [SerializeField] private TextMeshProUGUI workersCollectingWaterText;
    [SerializeField] private TextMeshProUGUI workersAmplifyingSunlightText;
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        UpdateDebugUI();
        UpdateWorkerUI();
    }
    #endregion

    #region Private Methods
    private void UpdateDebugUI()
    {
        UpdateText(weatherTypeText, "Weather Type: ", WeatherSystem.Instance.GetCurrentWeather().ToString());
        UpdateText(weatherTimeRemainingText, "Time Remaining: ", WeatherSystem.Instance.GetWeatherTimeRemaining().ToString("F1") + "s");
        UpdateText(sunlightLevelText, "Sunlight Level: ", ResourceManagementSystem.Instance.GetSunlightLevel().ToString("F1"));
        UpdateText(waterLevelText, "Water Level: ", ResourceManagementSystem.Instance.GetWaterLevel().ToString("F1"));
        UpdateText(nutrientLevelText, "Nutrient Level: ", ResourceManagementSystem.Instance.GetNutrientLevel().ToString("F1"));
        UpdateText(plantGrowthText, "Plant Growth: ", plantGrowthSystem.GetCurrentGrowth().ToString("F1"));
        UpdateText(plantStageText, "Plant Stage: ", plantGrowthSystem.GetCurrentStage().ToString());
        UpdateText(plantHealthText, "Plant Health: ", plantHealthSystem.GetHealth().ToString("F1"));
        UpdateText(numberOfWeedsText, "Number of Weeds: ", WeedManager.Instance.GetTotalWeeds().ToString());
        UpdateText(numberOfWorkersText, "Number of Workers: ", WorkerManager.Instance.GetTotalWorkers().ToString());
    }

    private void UpdateWorkerUI()
    {
        WorkerManager workerManager = WorkerManager.Instance;
        UpdateText(totalWorkersText, "Total Workers: ", workerManager.GetTotalWorkers().ToString());
        UpdateText(idleWorkersText, "Idle Workers: ", workerManager.GetIdleWorkerCount().ToString());
        UpdateText(workersClearingWeedsText, "Workers Clearing Weeds: ", workerManager.GetWorkersClearingWeeds().ToString());
        UpdateText(workersCollectingNutrientsText, "Workers Collecting Nutrients: ", workerManager.GetWorkersCollectingNutrients().ToString());
        UpdateText(workersCollectingWaterText, "Workers Collecting Water: ", workerManager.GetWorkersCollectingWater().ToString());
        UpdateText(workersAmplifyingSunlightText, "Workers Amplifying Sunlight: ", workerManager.GetWorkersAmplifyingSunlight().ToString());
    }
    #endregion

    #region Utility Methods
    private void UpdateText(TextMeshProUGUI textElement, string prefix, string value)
    {
        if (textElement != null)
        {
            textElement.text = prefix + value;
        }
    }
    #endregion
}
