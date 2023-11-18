using UnityEngine;

public class Weed : MonoBehaviour
{
    #region Private Fields
    private AIActionClearWeeds lockingAgent;
    private bool locked;
    #endregion

    #region Private Methods
    private void ClearWeed()
    {
        WeedManager.Instance.RemoveWeed(this);
        Destroy(this.gameObject);
    }
    #endregion

    #region Public Methods
    public void Interact()
    {
        ClearWeed();
    }

    public void Lock(AIActionClearWeeds agent)
    {
        locked = true;
        lockingAgent = agent;
    }

    public void Unlock()
    {
        locked = false;
        lockingAgent = null;
    }

    public bool IsLocked()
    {
        return locked;
    }

    public AIActionClearWeeds GetLockingAgent()
    {
        return lockingAgent;
    }
    #endregion
}
