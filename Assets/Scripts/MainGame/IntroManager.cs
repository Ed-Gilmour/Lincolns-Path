using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public string[] introTexts;
    private int currentIntroIndex;
    private bool canSkip;
    private float waitTime = 1f;

    private void Start()
    {
        StartCoroutine(IntroTextRoutine());
    }

    private void Update()
    {
        CheckForSkip();
    }

    private void CheckForSkip()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            canSkip = false;
        }
    }

    IEnumerator IntroTextRoutine()
    {
        introText.text = introTexts[currentIntroIndex];
        currentIntroIndex++;
        yield return new WaitForSeconds(waitTime);
        canSkip = true;

        if (currentIntroIndex >= introTexts.Length) yield break;

        yield return new WaitUntil(() => !canSkip);
        StartCoroutine(IntroTextRoutine());
    }
}