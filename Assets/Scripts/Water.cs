using UnityEngine;

public class Water : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float waterValue = 10f;
    #endregion

    #region Private Fields
    private AIActionCollectWater lockingAgent;
    private bool locked;
    #endregion

    #region Private Methods
    private void PickUpWater()
    {
        WaterManager.Instance.RemoveWater(this);
        Destroy(this.gameObject);
    }
    #endregion

    #region Public Methods
    public void Interact()
    {
        PickUpWater();
    }

    public void Lock(AIActionCollectWater agent)
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

    public AIActionCollectWater GetLockingAgent()
    {
        return lockingAgent;
    }

    public float GetWaterAmount()
    {
        return waterValue;
    }
    #endregion
}
