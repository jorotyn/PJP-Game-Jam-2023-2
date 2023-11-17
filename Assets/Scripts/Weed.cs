using UnityEngine;

public class Weed : MonoBehaviour
{
    #region Private Methods
    private void ClearWeed()
    {
        WeedManager.Instance.RemoveWeed();
        Destroy(this.gameObject);
    }
    #endregion

    #region Public Methods
    public void Interact()
    {
        ClearWeed();
    }
    #endregion
}
