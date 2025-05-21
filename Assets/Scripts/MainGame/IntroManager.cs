using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private MaskSwipeEffect maskSwipeEffect;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private string[] introTexts;
    private int currentIntroIndex;
    private bool canSkip;

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

    private IEnumerator IntroTextRoutine()
    {
        introText.text = introTexts[currentIntroIndex];
        currentIntroIndex++;

        maskSwipeEffect.SwipeEffectIn();

        yield return new WaitUntil(() => !maskSwipeEffect.swipingIn);

        canSkip = true;

        yield return new WaitUntil(() => !canSkip);

        maskSwipeEffect.SwipeEffectOut();

        yield return new WaitUntil(() => !maskSwipeEffect.swipingOut);

        if (currentIntroIndex < introTexts.Length)
        {
            StartCoroutine(IntroTextRoutine());
        }
    }
}