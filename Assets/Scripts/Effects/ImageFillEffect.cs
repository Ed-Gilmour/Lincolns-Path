using UnityEngine;
using UnityEngine.UI;

public class ImageFillEffect : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Color increaseColor;
    [SerializeField] private Color decreaseColor;
    [SerializeField] private float fillTime;
    [SerializeField] private float colorFadeTime;
    private float targetFill;
    private float fillChange;
    private float totalFillTime;
    private float targetColorFadeTime;
    private float colorTime;
    private Color originalColor;
    private bool isPlaying;

    private void Awake()
    {
        originalColor = fillImage.color;
        targetColorFadeTime = fillTime - colorFadeTime;
    }

    private void Update()
    {
        UpdateColorFade();
        UpdateImageFill();
    }

    private void UpdateColorFade()
    {
        if (isPlaying)
        {
            float reverseMult = totalFillTime >= targetColorFadeTime ? -1f : 1f;
            colorTime = Mathf.Clamp01(colorTime + Time.deltaTime / colorFadeTime * reverseMult);
            fillImage.color = Color.Lerp(originalColor, fillChange > 0f ? increaseColor : decreaseColor, colorTime);
        }
        else
        {
            fillImage.color = originalColor;
        }
    }

    private void UpdateImageFill()
    {
        if (isPlaying)
        {
            totalFillTime += Time.deltaTime;
            fillImage.fillAmount += Time.deltaTime * (fillChange / fillTime);
            if ((fillChange < 0f && fillImage.fillAmount < targetFill) || (fillChange > 0f && fillImage.fillAmount > targetFill))
            {
                fillImage.fillAmount = targetFill;
            }
            if (Mathf.Approximately(fillImage.fillAmount, targetFill))
            {
                isPlaying = false;
            }
        }
    }

    public void PlayFillAnimation(float change)
    {
        isPlaying = true;
        totalFillTime = 0f;
        fillChange = change;
        targetFill = fillImage.fillAmount + fillChange;
    }
}