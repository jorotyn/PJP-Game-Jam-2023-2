using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIActionClearWeeds : AIAction
{
    #region Serialized Fields
    [SerializeField] private float interactionRange = 1f;
    #endregion

    #region Private Fields
    private NavMeshAgent navMeshAgent;
    private Weed nearestWeed;
    #endregion

    #region Lifecycle
    public override void OnEnterState()
    {
        base.OnEnterState();
        nearestWeed = null;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        if (nearestWeed != null)
        {
            nearestWeed.Unlock();
            nearestWeed = null;
        }
    }
    #endregion

    #region Initialization
    public override void Initialization()
    {
        base.Initialization();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
    }
    #endregion

    #region Public Methods
    public override void PerformAction()
    {
        LocateNearestWeed();
        RemoveNearestWeed();
    }
    #endregion

    #region Private Methods
    private void LocateNearestWeed()
    {
        if (nearestWeed == null)
        {
            float nearestWeedDistance = Mathf.Infinity;
            List<Weed> allWeeds = WeedManager.Instance.GetAllWeedComponents();

            foreach (Weed weed in allWeeds)
            {
                if (weed != null && !weed.IsLocked())
                {
                    float distanceToWeed = Vector3.Distance(transform.position, weed.transform.position);
                    if (distanceToWeed < nearestWeedDistance)
                    {
                        nearestWeed = weed;
                        nearestWeedDistance = distanceToWeed;
                    }
                }
            }
        }

        if (nearestWeed != null)
        {
            if (nearestWeed.GetLockingAgent() == null)
            {
                nearestWeed.Lock(this);
                navMeshAgent.SetDestination(nearestWeed.transform.position);
            }
        }
    }

    private void RemoveNearestWeed()
    {
        if (nearestWeed != null)
        {
            float distanceToWeed = Vector3.Distance(transform.position, nearestWeed.transform.position);

            if (distanceToWeed <= interactionRange)
            {
                nearestWeed.Interact();
                nearestWeed.Unlock();
                nearestWeed = null;
                AudioManager.Instance.PlayWeedCut();
            }
        }
    }
    #endregion
}
