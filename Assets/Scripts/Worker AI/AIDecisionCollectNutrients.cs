using MoreMountains.Tools;

public class AIDecisionCollectNutrients : AIDecision
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
        return IsCollectNutrients();
    }
    #endregion

    #region Private Methods
    private bool IsCollectNutrients()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.CollectNutrients;
    }
    #endregion
}
