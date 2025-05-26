using UnityEngine;
using UnityEngine.UI;

public class ImageFillEffect : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Color increaseColor;
    [SerializeField] private Color decreaseColor;
    private float targetFill;

    public void PlayFillAnimation(float fillChange)
    {
        targetFill = fillImage.fillAmount + fillChange;
    }
}