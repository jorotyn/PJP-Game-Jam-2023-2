using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

public class WeedManager : MMSingleton<WeedManager>
{
    #region Serialized Fields
    [SerializeField] private GameObject weedPrefab;
    [SerializeField] private float baseGrowthRate = 1f;
    [SerializeField] private float minSpawnTime = 5f;
    [SerializeField] private float maxSpawnTime = 20f;
    [SerializeField] private float rainSpawnMultiplier = 1.5f;
    [SerializeField] private float sunSpawnMultiplier = 100f;
    [SerializeField] private float weedsSpawnMultiplier = 0.1f;
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;
    #endregion

    #region Private Fields
    private List<Weed> weedComponents = new List<Weed>();
    private int totalWeeds;
    private float currentGrowth;
    private float nextSpawnTime;
    private float sunlightLevel;
    private bool isRaining;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeWeeds();
    }

    private void Update()
    {
        GetWeatherType();
        GetSunlightLevel();
        UpdateWeedGrowth();
        CheckForWeedSpawn();
    }
    #endregion

    #region Initialization
    private void InitializeWeeds()
    {
        totalWeeds = 0;
        SetNextSpawnLevel();
    }
    #endregion

    #region Private Methods
    private void SetNextSpawnLevel(float multiplier = 1.0f)
    {
        float adjustedMinSpawnTime = minSpawnTime * multiplier;
        float adjustedMaxSpawnTime = maxSpawnTime * multiplier;

        nextSpawnTime = Random.Range(adjustedMinSpawnTime, adjustedMaxSpawnTime);
    }

    private void GetWeatherType()
    {
        isRaining = WeatherSystem.Instance.GetIsRaining();
    }

    private void GetSunlightLevel()
    {
        sunlightLevel = ResourceManagementSystem.Instance.GetSunlightLevel();
    }

    private void UpdateWeedGrowth()
    {
        float growthRate = CalculateGrowthRate();
        currentGrowth += growthRate * Time.deltaTime;
    }

    private void CheckForWeedSpawn()
    {
        if (currentGrowth >= nextSpawnTime)
        {
            SpawnWeed();
        }
    }

    private void SpawnWeed()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            0,
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        GameObject newWeedObject = Instantiate(weedPrefab, spawnPosition, Quaternion.identity, this.transform);
        Weed newWeedComponent = newWeedObject.GetComponent<Weed>();
        weedComponents.Add(newWeedComponent);
        totalWeeds++;
        currentGrowth = 0f;
        SetNextSpawnLevel();
    }
    #endregion

    #region Utility Methods
    private float CalculateGrowthRate()
    {
        float sunlightMultiplier = sunlightLevel / sunSpawnMultiplier;
        float rainMultiplier = (isRaining) ? rainSpawnMultiplier : 1f;
        float weedMultiplier = 1f + totalWeeds * weedsSpawnMultiplier;

        return baseGrowthRate * sunlightMultiplier * rainMultiplier * weedMultiplier;

    }
    #endregion

    #region Public Methods
    public void RemoveWeed(Weed weedToRemove)
    {
        if (weedComponents.Contains(weedToRemove))
        {
            weedComponents.Remove(weedToRemove);
            totalWeeds = Mathf.Max(0, totalWeeds - 1);
        }
    }

    public int GetTotalWeeds()
    {
        return totalWeeds;
    }

    public List<Weed> GetAllWeedComponents()
    {
        return weedComponents;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(spawnAreaMin.x, 0, spawnAreaMin.y), new Vector3(spawnAreaMax.x, 0, spawnAreaMin.y));
        Gizmos.DrawLine(new Vector3(spawnAreaMax.x, 0, spawnAreaMin.y), new Vector3(spawnAreaMax.x, 0, spawnAreaMax.y));
        Gizmos.DrawLine(new Vector3(spawnAreaMax.x, 0, spawnAreaMax.y), new Vector3(spawnAreaMin.x, 0, spawnAreaMax.y));
        Gizmos.DrawLine(new Vector3(spawnAreaMin.x, 0, spawnAreaMax.y), new Vector3(spawnAreaMin.x, 0, spawnAreaMin.y));
    }
    #endregion
}
