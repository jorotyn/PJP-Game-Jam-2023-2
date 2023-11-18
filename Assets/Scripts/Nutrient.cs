using UnityEngine;

public class Nutrient : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private float nutrientValue = 10f;
    #endregion

    #region Private Fields
    private AIActionCollectNutrients lockingAgent;
    private bool locked;
    #endregion

    #region Private Methods
    private void PickUpNutrient()
    {
        NutrientManager.Instance.RemoveNutrient(this);
        Destroy(this.gameObject);
    }
    #endregion

    #region Public Methods
    public void Interact()
    {
        PickUpNutrient();
    }

    public void Lock(AIActionCollectNutrients agent)
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

    public AIActionCollectNutrients GetLockingAgent()
    {
        return lockingAgent;
    }

    public float GetNutrientAmount()
    {
        return nutrientValue;
    }
    #endregion
}
