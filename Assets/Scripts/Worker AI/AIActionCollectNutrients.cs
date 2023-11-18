using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIActionCollectNutrients : AIAction
{
    #region Serialized Fields
    [SerializeField] private float interactionRange = 1f;
    #endregion

    #region Private Fields
    private NavMeshAgent navMeshAgent;
    private Nutrient nearestNutrient;
    private Transform plantTransform;
    private float nutrientsCollected;
    private bool nutrientCollected;
    #endregion

    #region Lifecycle
    public override void OnEnterState()
    {
        base.OnEnterState();
        nearestNutrient = null;
        nutrientCollected = false;
        nutrientsCollected = 0f;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        if (nearestNutrient != null)
        {
            nearestNutrient.Unlock();
            nearestNutrient = null;
        }

        if (nutrientCollected)
        {
            DropNutrient();
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
        LocateNearestNutrient();
        CollectNearestNutrient();
        TakeNutrientToPlant();
        DeliverNutrient();
    }
    #endregion

    #region Private Methods
    private void LocateNearestNutrient()
    {
        if (!nutrientCollected)
        {
            if (nearestNutrient == null)
            {
                float nearestNutrientDistance = Mathf.Infinity;
                List<Nutrient> allNutrients = NutrientManager.Instance.GetAllNutrientComponents();

                foreach (Nutrient nutrient in allNutrients)
                {
                    if (nutrient != null && !nutrient.IsLocked())
                    {
                        float distanceToNutrient = Vector3.Distance(transform.position, nutrient.transform.position);
                        if (distanceToNutrient < nearestNutrientDistance)
                        {
                            nearestNutrient = nutrient;
                            nearestNutrientDistance = distanceToNutrient;
                        }
                    }
                }
            }

            if (nearestNutrient != null)
            {
                if (nearestNutrient.GetLockingAgent() == null)
                {
                    nearestNutrient.Lock(this);
                    navMeshAgent.SetDestination(nearestNutrient.transform.position);
                }
            }
        }
    }

    private void CollectNearestNutrient()
    {
        if (!nutrientCollected && nearestNutrient != null)
        {
            float distanceToNutrient = Vector3.Distance(transform.position, nearestNutrient.transform.position);

            if (distanceToNutrient <= interactionRange)
            {
                nutrientsCollected = nearestNutrient.GetNutrientAmount();
                nearestNutrient.Interact();
                nearestNutrient.Unlock();
                nearestNutrient = null;
                nutrientCollected = true;
            }
        }
    }

    private void TakeNutrientToPlant()
    {
        if (nutrientCollected)
        {
            navMeshAgent.SetDestination(plantTransform.position);
        }
    }

    private void DeliverNutrient()
    {
        if (nutrientCollected)
        {
            float distanceToPlant = Vector3.Distance(transform.position, plantTransform.position);
            if (distanceToPlant <= interactionRange)
            {
                ResourceManagementSystem.Instance.AddNutrients(nutrientsCollected);
                nutrientCollected = false;
                nutrientsCollected = 0f;
            }
        }
    }

    private void DropNutrient()
    {
        NutrientManager.Instance.SpawnNutrient(transform.position);
        nutrientCollected = false;
        nutrientsCollected = 0f;
    }
    #endregion
}
