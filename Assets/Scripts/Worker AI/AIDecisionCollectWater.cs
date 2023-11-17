using MoreMountains.Tools;

public class AIDecisionCollectWater : AIDecision
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
        return IsCollectWater();
    }
    #endregion

    #region Private Methods
    private bool IsCollectWater()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.CollectWater;
    }
    #endregion
}
