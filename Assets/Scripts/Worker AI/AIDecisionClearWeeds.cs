using MoreMountains.Tools;

public class AIDecisionClearWeeds : AIDecision
{
    private Worker worker;

    public override void Initialization()
    {
        worker = GetComponentInParent<Worker>();
    }

    public override bool Decide()
    {
        return IsClearWeeds();
    }

    private bool IsClearWeeds()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.ClearWeeds;
    }
}
