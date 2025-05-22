using UnityEngine;
using TMPro;

public class TextFadeEffect : MonoBehaviour
{
    public TextMeshProUGUI effectText;
    [SerializeField] private float fadeTime;
    [SerializeField] private float pulseTime;
    [SerializeField] private float pulseDepth;
    private bool isFading;
    private bool isPulsing;

    private void Update()
    {
        UpdateFade();
    }

    private void UpdateFade()
    {
        if (isFading && !isPulsing)
        {
            UpdateTextAlpha(fadeTime);
            if (effectText.alpha >= 1f)
            {
                isPulsing = true;
            }
        }
        else if (isPulsing)
        {
            UpdateTextAlpha(pulseTime / pulseDepth, pulseDepth);
            if (effectText.alpha >= 1f || effectText.alpha <= pulseDepth)
            {
                pulseTime = -pulseTime;
            }
        }
        else if (!isFading && effectText.alpha > 0f)
        {
            UpdateTextAlpha(-fadeTime);
        }
    }

    private void UpdateTextAlpha(float time, float clampMin = 0f)
    {
        effectText.alpha = Mathf.Clamp(effectText.alpha + (Time.deltaTime / time), clampMin, 1f);
    }

    public void FadeIn()
    {
        isFading = true;
    }

    public void FadeOut()
    {
        isFading = false;
        isPulsing = false;
        pulseTime = Mathf.Abs(pulseTime);
    }
}