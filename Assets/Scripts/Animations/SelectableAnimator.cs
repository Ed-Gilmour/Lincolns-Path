using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string enterName;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetAnimator.SetBool(enterName, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetAnimator.SetBool(enterName, false);
    }
}