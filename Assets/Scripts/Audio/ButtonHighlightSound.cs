using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioManager.AudioClipData highlightSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.PlayAudioClip(highlightSound);
    }
}