using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.AI;

public class AIActionIdle : AIAction
{
    #region Serialized Fields
    [SerializeField] private float wanderRadius = 5f;
    [SerializeField] private float minWanderTimer = 0f;
    [SerializeField] private float maxWanderTimer = 5f;
    #endregion

    #region Private Fields
    private NavMeshAgent navMeshAgent;
    private float timer;
    private float wanderTimer;
    #endregion

    #region Initialization
    public override void Initialization()
    {
        base.Initialization();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        RandomizeWanderTimer();
    }
    #endregion

    #region Public Methods
    public override void PerformAction()
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
