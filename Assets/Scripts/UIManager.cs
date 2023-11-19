using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class UIManager : MMSingleton<UIManager>
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private PlantGrowthSystem plantGrowthSystem;
    [SerializeField] private PlantHealthSystem plantHealthSystem;

    [Header("Information Elements")]
    [SerializeField] private TextMeshProUGUI currentWeatherText;
    [SerializeField] private TextMeshProUGUI plantStageText;
    [SerializeField] private MMProgressBar growthBar;
    [SerializeField] private MMProgressBar healthBar;
    [SerializeField] private MMProgressBar sunlightBar;
    [SerializeField] private MMProgressBar waterBar;
    [SerializeField] private MMProgressBar nutrientBar;

    [Header("Worker Elements")]
    [SerializeField] private TextMeshProUGUI totalWorkersText;
    [SerializeField] private TextMeshProUGUI idleWorkersText;
    [SerializeField] private TextMeshProUGUI workersClearingWeedsText;
    [SerializeField] private TextMeshProUGUI workersCollectingNutrientsText;
    [SerializeField] private TextMeshProUGUI workersCollectingWaterText;
    [SerializeField] private TextMeshProUGUI workersAmplifyingSunlightText;

    [SerializeField] private GameObject workerPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject timePanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        workerPanel.SetActive(true);
        infoPanel.SetActive(true);
        timePanel.SetActive(true);
        losePanel.SetActive(false);
        winPanel.SetActive(false);
    }

    private void Update()
    {
        UpdateInformationUI();
        UpdateWorkerUI();
    }
    #endregion

    #region Private Methods
    private void UpdateInformationUI()
    {
        UpdateText(currentWeatherText, "Weather: ", WeatherSystem.Instance.GetCurrentWeather().ToString());
        int growthStage = plantGrowthSystem.GetCurrentStage() + 1;
        UpdateText(plantStageText, "Plant Stage: ", growthStage.ToString());
        growthBar.SetBar01(plantGrowthSystem.GetCurrentGrowthNormalized());
        healthBar.SetBar01(plantHealthSystem.GetHealth() / 100f);
        sunlightBar.SetBar01(ResourceManagementSystem.Instance.GetSunlightLevel() / 100f);
        waterBar.SetBar01(ResourceManagementSystem.Instance.GetWaterLevel() / 100f);
        nutrientBar.SetBar01(ResourceManagementSystem.Instance.GetNutrientLevel() / 100f);
    }

    private void UpdateWorkerUI()
    {
        WorkerManager workerManager = WorkerManager.Instance;
        UpdateText(totalWorkersText, "Total Workers: ", workerManager.GetTotalWorkers().ToString());
        UpdateText(idleWorkersText, "Idle Workers: ", workerManager.GetIdleWorkerCount().ToString());
        UpdateText(workersClearingWeedsText, "Workers Clearing Weeds: ", workerManager.GetWorkersClearingWeeds().ToString());
        UpdateText(workersCollectingNutrientsText, "Workers Collecting Nutrients: ", workerManager.GetWorkersCollectingNutrients().ToString());
        UpdateText(workersCollectingWaterText, "Workers Collecting Water: ", workerManager.GetWorkersCollectingWater().ToString());
        UpdateText(workersAmplifyingSunlightText, "Workers Collecting Sunlight: ", workerManager.GetWorkersAmplifyingSunlight().ToString());
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

    #region Public Methods
    public void ShowWin()
    {
        workerPanel.SetActive(false);
        infoPanel.SetActive(false);
        timePanel.SetActive(false);
        losePanel.SetActive(false);
        winPanel.SetActive(true);
    }

    public void ShowLose()
    {
        workerPanel.SetActive(false);
        infoPanel.SetActive(false);
        timePanel.SetActive(false);
        losePanel.SetActive(true);
        winPanel.SetActive(false);
    }
    #endregion
}
