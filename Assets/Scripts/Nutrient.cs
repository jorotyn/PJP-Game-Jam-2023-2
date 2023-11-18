using UnityEngine;

public class Nutrient : MonoBehaviour
{
    #region Private Fields
    private AIActionClearWeeds lockingAgent;
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
    #endregion
}
