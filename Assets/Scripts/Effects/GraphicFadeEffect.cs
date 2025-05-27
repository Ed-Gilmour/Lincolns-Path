using UnityEngine;
using UnityEngine.UI;

public class GraphicFadeEffect : MonoBehaviour
{
    [SerializeField] private Graphic targetGraphic;
    [SerializeField] private float fadeSpeed;
    private float fadeTime;
    private bool isFading;
    private Color normalColor;
    private Color fadeColor;

    private void Awake()
    {
        InitializeColors();
    }

    private void Update()
    {
        UpdateFade();
    }

    private void InitializeColors()
    {
        normalColor = new(targetGraphic.color.r, targetGraphic.color.g, targetGraphic.color.b, 1f);
        fadeColor = new(targetGraphic.color.r, targetGraphic.color.g, targetGraphic.color.b, 0f);
    }

    private void UpdateFade()
    {
        if (isFading)
        {
            fadeTime = Mathf.Clamp01(fadeTime + (Time.deltaTime / fadeSpeed));
            targetGraphic.color = Color.Lerp(fadeColor, normalColor, fadeTime);
            if (fadeTime >= 1f || fadeTime <= 0f)
            {
                isFading = false;
            }
        }
    }

    public void SetFade(bool fadeIn)
    {
        isFading = true;
        fadeSpeed = Mathf.Abs(fadeSpeed) * (fadeIn ? 1f : -1f);
    }
}