using MoreMountains.Tools;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private MMProgressBar sunlightBar;
    [SerializeField] private MMProgressBar waterBar;
    [SerializeField] private MMProgressBar nutrientBar;
    #endregion

    #region Unity Lifecycle
    private void Update()
    {
        UpdateResourceUI();
    }
    #endregion

    #region Private Methods
    private void UpdateResourceUI()
    {
        sunlightBar.SetBar01(ResourceManagementSystem.Instance.GetSunlightLevel() / 100f);
        waterBar.SetBar01(ResourceManagementSystem.Instance.GetWaterLevel() / 100f);
        nutrientBar.SetBar01(ResourceManagementSystem.Instance.GetNutrientLevel() / 100f);
    }
    #endregion
}
