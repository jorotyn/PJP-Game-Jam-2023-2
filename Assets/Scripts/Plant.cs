using MoreMountains.Tools;
using UnityEngine;

public class Plant : MMSingleton<Plant>
{
    #region Public Methods
    public Transform GetPlantLocation()
    {
        return this.transform;
    }
    #endregion
}
