using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private float characterDelay;
    private string targetString;

    public float PlayTypewriterEffect(string newText)
    {
        effectText.text = string.Empty;
        targetString = newText;
        StartCoroutine(TypewriterEffectRoutine());
        return (targetString.Length - 1) * characterDelay;
    }

    private IEnumerator TypewriterEffectRoutine()
    {
        effectText.text += targetString[0];
        targetString = targetString[1..];

        if (targetString.Length == 0) yield break;

        yield return new WaitForSeconds(characterDelay);
        StartCoroutine(TypewriterEffectRoutine());
    }
}