using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [Range(0f, 1f)] private float fillAmount = 0f;

    private float previousFillAmount = -1f; // sentinel to force first update

    // Called when you want to set progress externally
    public void SetProgress(float value)
    {
        float clamped = Mathf.Clamp01(value);

        // Only update if the value actually changed
        if (!Mathf.Approximately(clamped, previousFillAmount))
        {
            fillAmount = clamped;
            fillImage.fillAmount = fillAmount;
            previousFillAmount = fillAmount;
        }
    }

    // Optional: for initial setup
    private void Start()
    {
        fillAmount = 0f;
        SetProgress(fillAmount);
    }
}
