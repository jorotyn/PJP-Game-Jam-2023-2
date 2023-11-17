using MoreMountains.Tools;

public class AIDecisionAmplifySunlight : AIDecision
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
        return IsAmplifySunlight();
    }
    #endregion

    #region Private Methods
    private bool IsAmplifySunlight()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.AmplifySunlight;
    }
    #endregion
}
