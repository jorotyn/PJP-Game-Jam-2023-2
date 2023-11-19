using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class TimeScaleHandler : MMSingleton<TimeScaleHandler>
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private TextMeshProUGUI timeScaleText;

    [Header("Settings")]
    [SerializeField] private float[] allowedTimeScales = { 1f, 2f, 5f, 10f };
    #endregion

    #region Private Fields
    private float targetTimeScale = 1f;
    private bool isPaused = false;
    #endregion

    #region Public Methods
    public void DecreaseTimeScale()
    {
        // Find the current index of the targetTimeScale in the list of allowed values.
        int currentIndex = System.Array.IndexOf(allowedTimeScales, targetTimeScale);

        // If the current value is not the smallest, set the targetTimeScale to the next lowest value.
        if (currentIndex > 0)
        {
            targetTimeScale = allowedTimeScales[currentIndex - 1];
            timeScaleText.text = $"{targetTimeScale}x";
            if (!isPaused) MMTimeManager.Instance.SetTimeScaleTo(targetTimeScale);
        }
    }

    public void IncreaseTimeScale()
    {
        // Find the current index of the targetTimeScale in the list of allowed values.
        int currentIndex = System.Array.IndexOf(allowedTimeScales, targetTimeScale);

        // If the current value is not the largest, set the targetTimeScale to the next highest value.
        if (currentIndex < allowedTimeScales.Length - 1)
        {
            targetTimeScale = allowedTimeScales[currentIndex + 1];
            timeScaleText.text = $"{targetTimeScale}x";
            if (!isPaused) MMTimeManager.Instance.SetTimeScaleTo(targetTimeScale);
        }
    }

    public void PauseTime()
    {
        MMTimeManager.Instance.SetTimeScaleTo(0f);
        isPaused = true;
    }

    public void ResumeTime()
    {
        MMTimeManager.Instance.SetTimeScaleTo(targetTimeScale);
        isPaused = false;
    }
    #endregion
}
