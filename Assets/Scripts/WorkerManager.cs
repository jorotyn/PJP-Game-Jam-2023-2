using MoreMountains.Tools;

public class WorkerManager : MMSingleton<WorkerManager>
{
    #region Private Fields
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

    #region Public Methods
    public void AssignWorkerToWeeding()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersClearingWeeds++;
        }
    }

    public void RemoveWorkerFromWeeding()
    {
        if (workersClearingWeeds > 0)
        {
            workersClearingWeeds--;
        }
    }

    public void AssignWorkerToCollectingNutrients()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersCollectingNutrients++;
        }
    }

    public void RemoveWorkerFromCollectingNutrients()
    {
        if (workersCollectingNutrients > 0)
        {
            workersCollectingNutrients--;
        }
    }

    public void AssignWorkerToCollectingWater()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersCollectingWater++;
        }
    }

    public void RemoveWorkerFromCollectingWater()
    {
        if (workersCollectingWater > 0)
        {
            workersCollectingWater--;
        }
    }

    public void AssignWorkerToAmplifyingSunlight()
    {
        if (GetIdleWorkerCount() > 0)
        {
            workersAmplifyingSunlight++;
        }
    }

    public void RemoveWorkerFromAmplifyingSunlight()
    {
        if (workersAmplifyingSunlight > 0)
        {
            workersAmplifyingSunlight--;
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
