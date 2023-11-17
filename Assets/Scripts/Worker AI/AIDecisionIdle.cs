using MoreMountains.Tools;

public class AIDecisionIdle : AIDecision
{
    private Worker worker;

    public override void Initialization()
    {
        worker = GetComponentInParent<Worker>();
    }

    public override bool Decide()
    {
        return IsIdle();
    }

    private bool IsIdle()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.Idle;
    }
}
