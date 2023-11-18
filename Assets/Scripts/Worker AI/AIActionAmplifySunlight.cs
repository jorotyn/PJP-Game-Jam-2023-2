using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIActionAmplifySunlight : AIAction
{
    #region Serialized Fields
    [SerializeField] private float interactionRange = 1f;
    [SerializeField] private float maxSunlightCapacity = 100f;
    [SerializeField] private float depositRate = 1f;
    #endregion

    #region Private Fields
    private NavMeshAgent navMeshAgent;
    private Transform plantTransform;
    private float sunlightCollected;
    private bool depositingSunlight;
    private Coroutine sunlightCoroutine;
    #endregion

    #region Lifecycle
    public override void OnEnterState()
    {
        base.OnEnterState();
        depositingSunlight = false;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        if (sunlightCoroutine != null)
        {
            StopCoroutine(sunlightCoroutine);
            ResourceManagementSystem.Instance.DecrementSunlightAmplifiers();
        }
        depositingSunlight = false;
        sunlightCoroutine = null;
    }
    #endregion

    #region Initialization
    public override void Initialization()
    {
        base.Initialization();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        plantTransform = Plant.Instance.GetPlantLocation();
    }
    #endregion

    #region Public Methods
    public override void PerformAction()
    {
        CollectSunlight();
        TakeSunlightToPlant();
        DeliverSunlight();
    }
    #endregion

    #region Private Methods
    private void CollectSunlight()
    {
        if (sunlightCollected < maxSunlightCapacity && !depositingSunlight)
        {
            float sunlightLevel = ResourceManagementSystem.Instance.GetSunlightLevel();

            sunlightCollected += sunlightLevel / 100f * Time.deltaTime;
        }
    }

    private void TakeSunlightToPlant()
    {
        if (sunlightCollected >= maxSunlightCapacity)
        {
            navMeshAgent.SetDestination(plantTransform.position);
        }
    }

    private void DeliverSunlight()
    {
        if (sunlightCollected >= maxSunlightCapacity)
        {
            float distanceToPlant = Vector3.Distance(transform.position, plantTransform.position);
            if (distanceToPlant <= interactionRange)
            {
                depositingSunlight = true;
                if (sunlightCoroutine == null)
                {
                    sunlightCoroutine = StartCoroutine(ReduceSunlightOverTime());
                    ResourceManagementSystem.Instance.IncrementSunlightAmplifiers();
                }
            }
        }
    }

    private IEnumerator ReduceSunlightOverTime()
    {
        while (sunlightCollected > 0f)
        {
            sunlightCollected -= depositRate * Time.deltaTime;
            yield return null;
        }

        ResourceManagementSystem.Instance.DecrementSunlightAmplifiers();
        depositingSunlight = false;
        sunlightCoroutine = null;
    }
    #endregion
}
