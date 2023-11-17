using MoreMountains.Tools;
using System.Collections.Generic;

public class WorkerManager : MMSingleton<WorkerManager>
{
    #region Private Fields
    private Dictionary<Worker, Worker.WorkerTask> workerTasks;
    private int totalWorkers;
    private int workersClearingWeeds;
    private int workersCollectingNutrients;
    private int workersCollectingWater;
    private int workersAmplifyingSunlight;
    #endregion

    #region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();
        InitializeWorkerCounts();
        workerTasks = new Dictionary<Worker, Worker.WorkerTask>();
    }
    #endregion

    #region Initialization
    private void InitializeWorkerCounts()
    {
        workersClearingWeeds = 0;
        workersCollectingNutrients = 0;
        workersCollectingWater = 0;
        workersAmplifyingSunlight = 0;
    }
    #endregion

    #region Private Methods
    private void ReassignWorkerToTask(Worker.WorkerTask task)
    {
        foreach (var workerEntry in workerTasks)
        {
            if (workerEntry.Value == Worker.WorkerTask.Idle)
            {
                UpdateWorkerTask(workerEntry.Key, task);
                return;
            }
        }
    }

    private void ReassignWorkerToIdle(Worker.WorkerTask task)
    {
        foreach (var workerEntry in workerTasks)
        {
            if (workerEntry.Value == task)
            {
                UpdateWorkerTask(workerEntry.Key, Worker.WorkerTask.Idle);
                return;
            }
        }
    }
    #endregion

    #region Public Methods
    public void RegisterWorker(Worker worker)
    {
        if (!workerTasks.ContainsKey(worker))
        {
            workerTasks.Add(worker, Worker.WorkerTask.Idle);
            totalWorkers++;
        }
    }

    public void UnregisterWorker(Worker worker)
    {
        if (workerTasks.ContainsKey(worker))
        {
            workerTasks.Remove(worker);
            totalWorkers--;
        }
    }

    public void UpdateWorkerTask(Worker worker, Worker.WorkerTask newTask)
    {
        if (workerTasks.ContainsKey(worker))
        {
            workerTasks[worker] = newTask;
        }
        else
        {
            // Optionally handle the case where the worker is not registered
        }
    }

    public Worker.WorkerTask GetAssignedTaskForWorker(Worker worker)
    {
        if (workerTasks.ContainsKey(worker))
        {
            return workerTasks[worker];
        }

        return Worker.WorkerTask.Idle;
    }

    public void AssignWorkerToWeeding()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersClearingWeeds++;
            ReassignWorkerToTask(Worker.WorkerTask.ClearWeeds);
        }
    }

    public void RemoveWorkerFromWeeding()
    {
        if (workersClearingWeeds > 0)
        {
            workersClearingWeeds--;
            ReassignWorkerToIdle(Worker.WorkerTask.ClearWeeds);
        }
    }

    public void AssignWorkerToCollectingNutrients()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersCollectingNutrients++;
            ReassignWorkerToTask(Worker.WorkerTask.CollectNutrients);
        }
    }

    public void RemoveWorkerFromCollectingNutrients()
    {
        if (workersCollectingNutrients > 0)
        {
            workersCollectingNutrients--;
            ReassignWorkerToIdle(Worker.WorkerTask.CollectNutrients);
        }
    }

    public void AssignWorkerToCollectingWater()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersCollectingWater++;
            ReassignWorkerToTask(Worker.WorkerTask.CollectWater);
        }
    }

    public void RemoveWorkerFromCollectingWater()
    {
        if (workersCollectingWater > 0)
        {
            workersCollectingWater--;
            ReassignWorkerToIdle(Worker.WorkerTask.CollectWater);
        }
    }

    public void AssignWorkerToAmplifyingSunlight()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersAmplifyingSunlight++;
            ReassignWorkerToTask(Worker.WorkerTask.AmplifySunlight);
        }
    }

    public void RemoveWorkerFromAmplifyingSunlight()
    {
        if (workersAmplifyingSunlight > 0)
        {
            workersAmplifyingSunlight--;
            ReassignWorkerToIdle(Worker.WorkerTask.AmplifySunlight);
        }
    }

    public int GetIdleWorkerCount()
    {
        return totalWorkers - (workersClearingWeeds + workersCollectingNutrients + workersCollectingWater + workersAmplifyingSunlight);
    }

    public int GetTotalWorkers()
    {
        return totalWorkers;
    }

    public int GetWorkersClearingWeeds()
    {
        return workersClearingWeeds;
    }

    public int GetWorkersCollectingNutrients()
    {
        return workersCollectingNutrients;
    }

    public int GetWorkersCollectingWater()
    {
        return workersCollectingWater;
    }

    public int GetWorkersAmplifyingSunlight()
    {
        return workersAmplifyingSunlight;
    }
    #endregion
}
