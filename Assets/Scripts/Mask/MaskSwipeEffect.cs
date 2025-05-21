using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaskSwipeEffect : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectMask2D rectMask;
    [SerializeField] private TextMeshProUGUI maskedText;
    [SerializeField] private float fadeInSpeed;
    [SerializeField] private float fadeOutSpeed;
    [HideInInspector] public bool swipingIn;
    [HideInInspector] public bool swipingOut;

    private void Update()
    {
        UpdateSwipeEffect();
    }

    private void UpdateSwipeEffect()
    {
        if (swipingIn)
        {
            UpdateMaskRightPadding(rectMask.padding.z - (Time.deltaTime * fadeInSpeed));

            if (rectMask.padding.z <= GetLowEdgeDist())
            {
                swipingIn = false;
            }
        }
        else if (swipingOut)
        {
            UpdateMaskLeftPadding(rectMask.padding.x + (Time.deltaTime * fadeOutSpeed));

            if (rectMask.padding.x >= GetHighEdgeDist())
            {
                swipingOut = false;
            }
        }
    }

    public void SwipeEffectIn()
    {
        maskedText.ForceMeshUpdate();
        UpdateMaskLeftPadding(GetLowEdgeDist());
        UpdateMaskRightPadding(GetHighEdgeDist());
        swipingIn = true;
    }

    public void SwipeEffectOut()
    {
        swipingOut = true;
    }

    private float GetLowEdgeDist()
    {
        return (rectTransform.sizeDelta.x / 2f) - (rectMask.softness.x / 2f) - (maskedText.renderedWidth / 2f);
    }

    private float GetHighEdgeDist()
    {
        return (rectTransform.sizeDelta.x / 2f) + (maskedText.renderedWidth / 2f);
    }

    private void UpdateMaskRightPadding(float newPadding)
    {
        rectMask.padding = new Vector4(
            rectMask.padding.x,
            rectMask.padding.y,
            newPadding,
            rectMask.padding.w
            );
    }

    private void UpdateMaskLeftPadding(float newPadding)
    {
        rectMask.padding = new Vector4(
            newPadding,
            rectMask.padding.y,
            rectMask.padding.z,
            rectMask.padding.w
            );
    }
}