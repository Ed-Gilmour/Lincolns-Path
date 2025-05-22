using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private MaskSwipeEffect maskSwipeEffect;
    [SerializeField] private TextFadeEffect continueTextFade;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private AudioManager.AudioClipData swipeOutSound;
    [SerializeField] private IntroTextData[] introData;
    private int currentIntroIndex;
    private bool canSkip;
    private bool continueTextActive;

    [Serializable]
    class IntroTextData
    {
        public AudioManager.AudioClipData textSound;
        public string introText;
        public float continueTextTime;
    }

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
            if (continueTextActive)
            {
                continueTextFade.FadeOut();
                continueTextActive = false;
            }
        }
    }

    private IEnumerator IntroTextRoutine()
    {
        IntroTextData textData = introData[currentIntroIndex];
        introText.text = textData.introText;
        currentIntroIndex++;

        maskSwipeEffect.SwipeEffectIn();
        textData.textSound.Play();

        yield return new WaitUntil(() => !maskSwipeEffect.swipingIn);

        canSkip = true;
        StartCoroutine(ContinueTextRoutine(currentIntroIndex, textData.continueTextTime));

        yield return new WaitUntil(() => !canSkip);

        maskSwipeEffect.SwipeEffectOut();
        swipeOutSound.Play();

        yield return new WaitUntil(() => !maskSwipeEffect.swipingOut);

        if (currentIntroIndex < introData.Length)
        {
            StartCoroutine(IntroTextRoutine());
        }
    }

    private IEnumerator ContinueTextRoutine(int tempIndex, float textTime)
    {
        yield return new WaitForSeconds(textTime);
        if (canSkip && tempIndex == currentIntroIndex)
        {
            continueTextActive = true;
            continueTextFade.FadeIn();
        }
    }
}