using MoreMountains.Tools;

public class AIDecisionAmplifySunlight : AIDecision
{
    private Worker worker;

    public override void Initialization()
    {
        worker = GetComponentInParent<Worker>();
    }

    public override bool Decide()
    {
        return IsAmplifySunlight();
    }

    private bool IsAmplifySunlight()
    {
        return worker.GetCurrentTask() == Worker.WorkerTask.AmplifySunlight;
    }
}
