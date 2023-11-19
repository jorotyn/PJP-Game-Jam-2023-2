using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIActionClearWeeds : AIAction
{
    #region Serialized Fields
    [SerializeField] private float interactionRange = 1f;

    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float minWanderTimer = 0f;
    [SerializeField] private float maxWanderTimer = 5f;
    #endregion

    #region Private Fields
    private NavMeshAgent navMeshAgent;
    private Weed nearestWeed;

    private float timer;
    private float wanderTimer;
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

        if (nearestWeed == null)
        {
            Wander();
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

    private void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            navMeshAgent.SetDestination(newPos);
            timer = 0;
            RandomizeWanderTimer();
        }
    }
    #endregion

    #region Utility Methods
    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void RandomizeWanderTimer()
    {
        wanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
    }
    #endregion
}
