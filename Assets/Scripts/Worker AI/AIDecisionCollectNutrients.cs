using MoreMountains.Tools;

public class AIDecisionCollectNutrients : AIDecision
{
    private Worker worker;

    public override void Initialization()
    {
        worker = GetComponentInParent<Worker>();
    }

    public override bool Decide()
    {
        return IsCollectNutrients();
    }

    private bool IsCollectNutrients()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.CollectNutrients;
    }
}
