using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioManager.AudioClipData highlightSound;
    [SerializeField] private AudioManager.AudioClipData clickSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightSound.Play();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }
}