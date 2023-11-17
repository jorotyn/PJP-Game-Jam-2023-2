using MoreMountains.Tools;

public class AIDecisionIdle : AIDecision
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
        return IsIdle();
    }
    #endregion

    #region Private Methods
    private bool IsIdle()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.Idle;
    }
    #endregion
}
