using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Resource Elements")]
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
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        UpdateResourceUI();
        UpdateWorkerUI();
    }
    #endregion

    #region Private Methods
    private void UpdateResourceUI()
    {
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
}
