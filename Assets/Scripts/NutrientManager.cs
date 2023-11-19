using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientManager : MMSingleton<NutrientManager>
{
    #region Serialized Fields
    [SerializeField] private GameObject nutrientPrefab;
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;
    [SerializeField] private int initialNutrients = 10;
    [SerializeField] private float spawnInterval = 5f;
    #endregion

    #region Private Fields
    private List<Nutrient> nutrientComponents = new List<Nutrient>();
    private int totalNutrients;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeNutrients();
        StartCoroutine(SpawnNutrientsOverTime());
    }
    #endregion

    #region Initialization
    private void InitializeNutrients()
    {
        totalNutrients = 0;

        for (int i = 0; i < initialNutrients; i++)
        {
            Vector3 spawnPosition = RandomPosition();
            SpawnNutrient(spawnPosition);
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator SpawnNutrientsOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Vector3 spawnPosition = RandomPosition();
            SpawnNutrient(spawnPosition);
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
    public void SpawnNutrient(Vector3 spawnPosition)
    {
        GameObject newNutrientObject = Instantiate(nutrientPrefab, spawnPosition, Quaternion.identity, this.transform);
        Nutrient newNutrientComponent = newNutrientObject.GetComponent<Nutrient>();
        nutrientComponents.Add(newNutrientComponent);
        totalNutrients++;
    }

    public void RemoveNutrient(Nutrient nutrientToRemove)
    {
        if (nutrientComponents.Contains(nutrientToRemove))
        {
            nutrientComponents.Remove(nutrientToRemove);
            totalNutrients = Mathf.Max(0, totalNutrients - 1);
        }
    }

    public int GetTotalNutrients()
    {
        return totalNutrients;
    }

    public List<Nutrient> GetAllNutrientComponents()
    {
        return nutrientComponents;
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
