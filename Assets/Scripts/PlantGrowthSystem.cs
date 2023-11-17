using UnityEngine;

public class PlantGrowthSystem : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private GameObject[] growthStagesModels;
    [SerializeField] private float baseGrowthRate;
    [SerializeField] private float sunlightRequirement;
    [SerializeField] private float waterRequirement;
    [SerializeField] private float nutrientRequirement;
    [SerializeField] private float weedGrowthPenalty;
    #endregion

    #region Private Fields
    private int currentStage;
    private float currentGrowth;
    private float sunlightLevel;
    private float waterLevel;
    private float nutrientLevel;
    private int numberOfWeeds;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateGrowth();
        CheckForStageTransition();
    }
    #endregion

    #region Initialization
    private void Initialize()
    {
        currentStage = 0;
        currentGrowth = 0f;
        sunlightLevel = 100f;
        waterLevel = 100f;
        nutrientLevel = 100f;
        numberOfWeeds = 0;

        UpdatePlantAppearance();
    }
    #endregion

    #region Private Methods
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
        }
    }

    private void TransitionToNextStage()
    {
        currentStage++;
        currentGrowth = 0f;
        UpdatePlantAppearance();

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

        return baseGrowthRate * sunlightMultiplier * waterMultiplier * nutrientMultiplier * weedPenalty;
    }

    private float GetGrowthThresholdForStage(int stage)
    {
        return 100f * (stage + 1);
    }
    #endregion

    #region Public Methods
    public void ModifyResourceLevels(float sunlightChange, float waterChange, float nutrientChange)
    {
        sunlightLevel += sunlightChange;
        waterLevel += waterChange;
        nutrientLevel += nutrientChange;
    }

    public void AddWeed()
    {
        numberOfWeeds++;
    }

    public void RemoveWeed()
    {
        numberOfWeeds = Mathf.Max(0, numberOfWeeds - 1);
    }
    #endregion
}
