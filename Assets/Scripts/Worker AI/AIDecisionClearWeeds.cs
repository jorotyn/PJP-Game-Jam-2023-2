using MoreMountains.Tools;

public class AIDecisionClearWeeds : AIDecision
{
    #region Private Fields
    private Worker worker;
    #endregion

    #region Initialization
    public override void Initialization()
    {
        worker = GetComponentInParent<Worker>();
    }
    #endregion

    #region Public Methods
    public override bool Decide()
    {
        return IsClearWeeds();
    }
    #endregion

    #region Private Methods
    private bool IsClearWeeds()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.ClearWeeds;
    }
    #endregion
}
