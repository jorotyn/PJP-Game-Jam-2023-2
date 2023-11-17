using UnityEngine;

public class Worker : MonoBehaviour
{
    public enum WorkerTask { Idle, ClearWeeds, CollectNutrients, CollectWater, AmplifySunlight }

    #region Private Fields
    private WorkerTask currentTask = WorkerTask.Idle;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        WorkerManager.Instance.RegisterWorker(this);
    }

    private void Update()
    {
        CheckAndUpdateTask();
    }

    private void OnDestroy()
    {
        WorkerManager.Instance.UnregisterWorker(this);
    }
    #endregion

    #region Private Methods
    private void CheckAndUpdateTask()
    {
        WorkerTask assignedTask = WorkerManager.Instance.GetAssignedTaskForWorker(this);
        if (currentTask != assignedTask)
        {
            UpdateTask(assignedTask);
        }
    }

    public void UpdateTask(WorkerTask newTask)
    {
        if (currentTask != newTask)
        {
            currentTask = newTask;
            WorkerManager.Instance.UpdateWorkerTask(this, newTask);
        }
    }
    #endregion

    #region Public Methods
    public WorkerTask GetCurrentTask()
    {
        return currentTask;
    }
    #endregion
}
