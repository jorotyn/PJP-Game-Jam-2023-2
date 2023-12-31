using UnityEngine;
using UnityEngine.Events;

public class PlantGrowthSystem : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private GameObject[] growthStagesModels;
    [SerializeField] private float baseGrowthRate;
    [SerializeField] private float sunlightRequirement;
    [SerializeField] private float waterRequirement;
    [SerializeField] private float nutrientRequirement;
    [SerializeField] private float weedGrowthPenalty;

    [SerializeField] private UnityEvent onGrowthStageIncrease;
    [SerializeField] private UnityEvent onFullyGrown;
    #endregion

    #region Private Fields
    private int currentStage;
    private float currentGrowth;
    private float sunlightLevel;
    private float waterLevel;
    private float nutrientLevel;
    private int numberOfWeeds;
    private int numberOfSunlightAmplifiers;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        GetResourceLevels();
        UpdateGrowth();
        CheckForStageTransition();
    }
    #endregion

    #region Initialization
    private void Initialize()
    {
        currentStage = 0;
        currentGrowth = 0f;
        numberOfWeeds = 0;
        numberOfSunlightAmplifiers = 0;

        UpdatePlantAppearance();
    }
    #endregion

    #region Private Methods
    private void GetResourceLevels()
    {
        sunlightLevel = ResourceManagementSystem.Instance.GetSunlightLevel();
        waterLevel = ResourceManagementSystem.Instance.GetWaterLevel();
        nutrientLevel = ResourceManagementSystem.Instance.GetNutrientLevel();
        numberOfSunlightAmplifiers = ResourceManagementSystem.Instance.GetNumberOfSunlightAmplifiers();
    }

    private void UpdateGrowth()
    {
        float growthRate = CalculateGrowthRate();
        currentGrowth += growthRate * Time.deltaTime;
    }

    private void CheckForStageTransition()
    {
        if (currentGrowth >= GetGrowthThresholdForStage(currentStage))
        {
            TransitionToNextStage();
            // Check if the plant has reached its final stage
            if (currentStage == growthStagesModels.Length - 1)
            {
                onFullyGrown.Invoke();
            }
        }
    }

    private void TransitionToNextStage()
    {
        currentStage++;
        currentGrowth = 0f;
        UpdatePlantAppearance();
        onGrowthStageIncrease.Invoke();
        // To do: Trigger particle effects here
    }

    private void UpdatePlantAppearance()
    {
        for (int i = 0; i < growthStagesModels.Length; i++)
        {
            growthStagesModels[i].SetActive(i == currentStage);
        }
    }
    #endregion

    #region Utility Methods
    private float CalculateGrowthRate()
    {
        float sunlightMultiplier = (sunlightLevel >= sunlightRequirement) ? 1f : sunlightLevel / sunlightRequirement;
        float waterMultiplier = (waterLevel >= waterRequirement) ? 1f : waterLevel / waterRequirement;
        float nutrientMultiplier = (nutrientLevel >= nutrientRequirement) ? 1f : nutrientLevel / nutrientRequirement;
        float weedPenalty = 1f - (numberOfWeeds * weedGrowthPenalty);

        float amplifierEffect = 1f + numberOfSunlightAmplifiers;

        return baseGrowthRate * sunlightMultiplier * waterMultiplier * nutrientMultiplier * weedPenalty * amplifierEffect;
    }

    private float GetGrowthThresholdForStage(int stage)
    {
        return 100f * (stage + 1);
    }
    #endregion

    #region Public Methods
    public void AddWeed()
    {
        numberOfWeeds++;
    }

    public void RemoveWeed()
    {
        numberOfWeeds = Mathf.Max(0, numberOfWeeds - 1);
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public float GetCurrentGrowth()
    {
        return currentGrowth;
    }

    public float GetCurrentGrowthNormalized()
    {
        float growthThreshold = GetGrowthThresholdForStage(currentStage);
        return Mathf.Max(0, currentGrowth / growthThreshold);
    }
    #endregion
}
