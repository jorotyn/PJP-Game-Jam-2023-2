using MoreMountains.Tools;

public class AIDecisionCollectWater : AIDecision
{
    private Worker worker;

    public override void Initialization()
    {
        worker = GetComponentInParent<Worker>();
    }

    public override bool Decide()
    {
        return IsCollectWater();
    }

    private bool IsCollectWater()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.CollectWater;
    }
}
