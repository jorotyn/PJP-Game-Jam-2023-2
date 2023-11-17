using MoreMountains.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIActionIdle : AIAction
{
    #region Serialized Fields
    [Header("Idle Movement Settings")]
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;
    #endregion

    #region Private Fields
    private NavMeshAgent navMeshAgent;
    private bool isMoving;
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
        if (isMoving)
        {
            Debug.Log("Already moving, returning from PerformAction");
            return;
        }

        //Debug.Log("Performing Idle Action");
        //StartCoroutine(IdleRoutine());
    }
    #endregion

    #region Private Methods
    private IEnumerator IdleRoutine()
    {
        isMoving = true;
        Debug.Log("Starting IdleRoutine");

        // Move to a random destination
        MoveToRandomDestination();

        // Wait until the destination is reached
        yield return new WaitUntil(() => !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f);

        Debug.Log("Destination reached");

        // Wait for a random amount of time
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        Debug.Log($"Waited for {waitTime} seconds");

        isMoving = false;
    }

    private void MoveToRandomDestination()
    {
        int maxAttempts = 10; // Maximum number of attempts to find a valid destination
        int attempts = 0;
        bool validDestinationFound = false;
        NavMeshHit hit;

        while (!validDestinationFound && attempts < maxAttempts)
        {
            Vector3 randomDirection = Random.insideUnitSphere * navMeshAgent.stoppingDistance;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out hit, navMeshAgent.stoppingDistance, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position);
                Debug.Log($"Moving to new destination: {hit.position}");
                validDestinationFound = true;
            }
            else
            {
                Debug.Log("Trying to find a valid destination");
                attempts++;
            }
        }

        if (!validDestinationFound)
        {
            Debug.Log("Failed to find a valid destination after maximum attempts");
            // Handle the case where no valid destination is found. For example:
            // - Trigger another action
            // - Move to a default position
            // - Log an error or warning
        }
    }
    #endregion
}
