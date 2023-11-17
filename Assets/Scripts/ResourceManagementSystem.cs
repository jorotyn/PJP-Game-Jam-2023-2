using MoreMountains.Tools;
using UnityEngine;

public class ResourceManagementSystem : MMSingleton<ResourceManagementSystem>
{
    #region Serialized Fields
    [SerializeField] private float waterConsumptionRate = 1f;
    [SerializeField] private float nutrientConsumptionRate = 1f;
    [SerializeField] private float maxResourceLevel = 100f;
    #endregion

    #region Private Fields
    private float sunlightLevel;
    private float waterLevel;
    private float nutrientLevel;
    #endregion

    #region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Update()
    {
        UpdateResources();
    }
    #endregion

    #region Initialization
    private void Initialize()
    {
        sunlightLevel = 50f;
        waterLevel = 50f;
        nutrientLevel = 50f;
    }
    #endregion

    #region Private Methods
    private void UpdateResources()
    {
        UpdateSunlight();
        ConsumeResources();
    }

    private void UpdateSunlight()
    {
        sunlightLevel = Mathf.PingPong(Time.time, maxResourceLevel);
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
    #endregion
}
