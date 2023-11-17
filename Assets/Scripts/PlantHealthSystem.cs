using UnityEngine;
using UnityEngine.Events;

public class PlantHealthSystem : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float healthRegenerationRate = 5f;
    [SerializeField] private float healthDegenerationRate = 5f;
    [SerializeField] private float sunlightThreshold = 0f;
    [SerializeField] private float waterThreshold = 0f;
    [SerializeField] private float sunlightTimeThreshold = 10f;
    [SerializeField] private float waterTimeThreshold = 10f;

    [SerializeField] private UnityEvent onHealthDecreaseStart;
    [SerializeField] private UnityEvent onHealthDecreaseStop;
    [SerializeField] private UnityEvent onPlayerLose;
    #endregion

    #region Private Fields
    private float currentHealth;
    private float sunlightLevel;
    private float waterLevel;
    private bool isHealthDecreasing;
    private float sunlightTimer;
    private float waterTimer;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateHealth();
    }
    #endregion

    #region Initialization
    private void Initialize()
    {
        currentHealth = maxHealth;
        isHealthDecreasing = false;
        sunlightTimer = 0f;
        waterTimer = 0f;
    }
    #endregion

    #region Private Methods
    private void UpdateHealth()
    {
        bool isSunlightBelowThreshold = sunlightLevel <= sunlightThreshold;
        bool isWaterBelowThreshold = waterLevel <= waterThreshold;

        if (isSunlightBelowThreshold)
        {
            sunlightTimer += Time.deltaTime;
            if (sunlightTimer >= sunlightTimeThreshold)
            {
                DecreaseHealth();
            }
        }
        else
        {
            sunlightTimer = 0f;
        }

        if (isWaterBelowThreshold)
        {
            waterTimer += Time.deltaTime;
            if (waterTimer >= waterTimeThreshold)
            {
                DecreaseHealth();
            }
        }
        else
        {
            waterTimer = 0f;
        }

        if (!isSunlightBelowThreshold && !isWaterBelowThreshold)
        {
            IncreaseHealth();
        }

        CheckHealthStatus();
    }

    private void DecreaseHealth()
    {
        if (!isHealthDecreasing)
        {
            onHealthDecreaseStart.Invoke();
            isHealthDecreasing = true;
        }

        currentHealth -= healthDegenerationRate * Time.deltaTime;
    }

    private void IncreaseHealth()
    {
        if (isHealthDecreasing)
        {
            onHealthDecreaseStop.Invoke();
            isHealthDecreasing = false;
        }

        currentHealth = Mathf.Min(currentHealth + healthRegenerationRate * Time.deltaTime, maxHealth);
    }

    private void CheckHealthStatus()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onPlayerLose.Invoke();
        }
    }
    #endregion

    #region Public Methods
    public void SetSunlightLevel(float level)
    {
        sunlightLevel = level;
    }

    public void SetWaterLevel(float level)
    {
        waterLevel = level;
    }
    #endregion
}
