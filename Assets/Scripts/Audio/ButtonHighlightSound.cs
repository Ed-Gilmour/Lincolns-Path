using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioManager.AudioClipData highlightSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.PlayAudioClip(highlightSound);
    }
}