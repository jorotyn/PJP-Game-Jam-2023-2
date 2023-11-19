using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public enum WorkerTask { Idle, ClearWeeds, CollectNutrients, CollectWater, AmplifySunlight }

    #region Serialized Fields
    [SerializeField] private List<WorkerTask> taskKeys;
    [SerializeField] private List<GameObject> taskGameObjects;
    #endregion

    #region Private Fields
    private Dictionary<WorkerTask, GameObject> taskGameObjectMap;
    private WorkerTask currentTask = WorkerTask.Idle;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // Initialize the dictionary from serialized lists
        taskGameObjectMap = new Dictionary<WorkerTask, GameObject>();
        for (int i = 0; i < taskKeys.Count; i++)
        {
            if (i < taskGameObjects.Count)
            {
                taskGameObjectMap[taskKeys[i]] = taskGameObjects[i];
            }
        }
    }

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
            // Disable the current task GameObject
            if (taskGameObjectMap.ContainsKey(currentTask))
            {
                taskGameObjectMap[currentTask].SetActive(false);
            }

            currentTask = newTask;
            WorkerManager.Instance.UpdateWorkerTask(this, newTask);

            // Enable the new task GameObject
            if (taskGameObjectMap.ContainsKey(newTask))
            {
                taskGameObjectMap[newTask].SetActive(true);
            }
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
