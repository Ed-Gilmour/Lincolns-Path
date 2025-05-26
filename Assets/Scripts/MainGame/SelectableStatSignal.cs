using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableStatSignal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isDecision1;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StatManager.Instance.SetStatSignals(isDecision1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StatManager.Instance.ClearStatSignals();
    }
}