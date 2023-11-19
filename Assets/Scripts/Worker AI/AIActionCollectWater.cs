using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIActionCollectWater : AIAction
{
    #region Serialized Fields
    [SerializeField] private float interactionRange = 1f;
    #endregion

    #region Private Fields
    private NavMeshAgent navMeshAgent;
    private Water nearestWater;
    private Transform plantTransform;
    private float waterAmountCollected;
    private bool isWaterCollected;
    #endregion

    #region Lifecycle
    public override void OnEnterState()
    {
        base.OnEnterState();
        nearestWater = null;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        if (nearestWater != null)
        {
            nearestWater.Unlock();
            nearestWater = null;
        }
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
        LocateNearestWater();
        CollectNearestWater();
        TakeWaterToPlant();
        DeliverWater();
    }
    #endregion

    #region Private Methods
    private void LocateNearestWater()
    {
        if (!isWaterCollected)
        {
            if (nearestWater == null)
            {
                float nearestWaterDistance = Mathf.Infinity;
                List<Water> allWater = WaterManager.Instance.GetAllWaterComponents();

                foreach (Water water in allWater)
                {
                    if (water != null && !water.IsLocked())
                    {
                        float distanceToWater = Vector3.Distance(transform.position, water.transform.position);
                        if (distanceToWater < nearestWaterDistance)
                        {
                            nearestWater = water;
                            nearestWaterDistance = distanceToWater;
                        }
                    }
                }
            }

            if (nearestWater != null)
            {
                if (nearestWater.GetLockingAgent() == null)
                {
                    nearestWater.Lock(this);
                    navMeshAgent.SetDestination(nearestWater.transform.position);
                }
            }
        }
    }

    private void CollectNearestWater()
    {
        if (!isWaterCollected && nearestWater != null)
        {
            float distanceToWater = Vector3.Distance(transform.position, nearestWater.transform.position);

            if (distanceToWater <= interactionRange)
            {
                waterAmountCollected = nearestWater.GetWaterAmount();
                nearestWater.Interact();
                nearestWater.Unlock();
                nearestWater = null;
                isWaterCollected = true;
                AudioManager.Instance.PlayWaterCollect();
            }
        }
    }

    private void TakeWaterToPlant()
    {
        if (isWaterCollected)
        {
            navMeshAgent.SetDestination(plantTransform.position);
        }
    }

    private void DeliverWater()
    {
        if (isWaterCollected)
        {
            float distanceToPlant = Vector3.Distance(transform.position, plantTransform.position);
            if (distanceToPlant <= interactionRange)
            {
                ResourceManagementSystem.Instance.AddWater(waterAmountCollected);
                isWaterCollected = false;
                waterAmountCollected = 0f;
                AudioManager.Instance.PlayWaterDeposit();
            }
        }
    }
    #endregion
}
