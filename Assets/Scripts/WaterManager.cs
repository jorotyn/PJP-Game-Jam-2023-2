using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MMSingleton<WaterManager>
{
    #region Serialized Fields
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;
    [SerializeField] private int initialWater = 2;
    #endregion

    #region Private Fields
    private List<Water> waterComponents = new List<Water>();
    private int totalWater;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeWater();
    }
    #endregion

    #region Initialization
    private void InitializeWater()
    {
        totalWater = 0;

        for (int i = 0; i < initialWater; i++)
        {
            Vector3 spawnPosition = RandomPosition();
            SpawnWater(spawnPosition);
        }
    }
    #endregion

    #region Utility Methods
    private Vector3 RandomPosition()
    {
        return new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            0,
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );
    }
    #endregion

    #region Public Methods
    public void SpawnWater(Vector3 spawnPosition)
    {
        GameObject newWaterObject = Instantiate(waterPrefab, spawnPosition, Quaternion.identity, this.transform);
        Water newWaterComponent = newWaterObject.GetComponent<Water>();
        waterComponents.Add(newWaterComponent);
        totalWater++;
    }

    public void RemoveWater(Water waterToRemove)
    {
        if (waterComponents.Contains(waterToRemove))
        {
            waterComponents.Remove(waterToRemove);
            totalWater = Mathf.Max(0, totalWater - 1);
        }
    }

    public int GetTotalWater()
    {
        return totalWater;
    }

    public List<Water> GetAllWaterComponents()
    {
        return waterComponents;
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
