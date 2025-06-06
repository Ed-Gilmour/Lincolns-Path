using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    public static IntroManager Instance { get; private set; }
    [SerializeField] private MaskSwipeEffect maskSwipeEffect;
    [SerializeField] private TextFadeEffect continueTextFade;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private AudioManager.AudioClipData swipeOutSound;
    [SerializeField] private IntroTextData[] introData;
    [HideInInspector] public IntroTextData[] endData;
    [HideInInspector] public IntroTextData[] yearBreakData;
    private int endIndex;
    private int currentIntroIndex;
    [HideInInspector] public int yearBreakIndex;
    private bool canSkip;
    private bool continueTextActive;
    public event Action<bool> OnFinishedSwipeOut;

    [Serializable]
    public class IntroTextData
    {
        public AudioManager.AudioClipData textSound;
        [TextArea] public string introText;
        public float continueTextTime;

        public IntroTextData(AudioManager.AudioClipData textSound, string introText, float continueTextTime)
        {
            this.textSound = textSound;
            this.introText = introText;
            this.continueTextTime = continueTextTime;
        }
    }

    void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        SceneLoader.Instance.FadeOut();

        yield return new WaitForSeconds(1f);

        if (!PauseMenu.Instance.restarted)
        {
            StartCoroutine(IntroTextRoutine(introData[currentIntroIndex]));
        }
        else
        {
            EventManager.Instance.StartGame();
        }
    }

    private void Update()
    {
        CheckForSkip();
    }

    private void CheckForSkip()
    {
        if (!CursorManager.Instance.isOverSelectable && !PauseMenu.Instance.GetIsPauseMenuOpen() && Mouse.current.leftButton.wasPressedThisFrame)
        {
            canSkip = false;
            if (continueTextActive)
            {
                continueTextFade.FadeOut();
                continueTextActive = false;
            }
        }
    }

    public IEnumerator IntroTextRoutine(IntroTextData textData)
    {
        bool notFromIntro = currentIntroIndex >= introData.Length || PauseMenu.Instance.restarted;

        introText.text = textData.introText;

        if (!notFromIntro)
        {
            currentIntroIndex++;
        }
        else if (endData != null && endData.Length > 0)
        {
            endIndex++;
        }
        else if (yearBreakData != null && yearBreakData.Length > 0)
        {
            yearBreakIndex++;
        }

        maskSwipeEffect.SwipeEffectIn();
        textData.textSound?.Play();

        yield return new WaitUntil(() => !maskSwipeEffect.swipingIn);

        canSkip = true;
        StartCoroutine(ContinueTextRoutine(currentIntroIndex, textData.continueTextTime));

        yield return new WaitUntil(() => !canSkip);

        maskSwipeEffect.SwipeEffectOut();

        if (!notFromIntro || (yearBreakData != null && yearBreakData.Length > 0))
        {
            swipeOutSound.Play();
        }

        yield return new WaitUntil(() => !maskSwipeEffect.swipingOut);

        if (endData != null && endData.Length > 0 && endIndex < endData.Length)
        {
            StartCoroutine(IntroTextRoutine(endData[endIndex]));
        }
        else if (yearBreakData != null && yearBreakData.Length > 0 && yearBreakIndex < yearBreakData.Length)
        {
            StartCoroutine(IntroTextRoutine(yearBreakData[yearBreakIndex]));
        }
        else
        {
            OnFinishedSwipeOut?.Invoke(notFromIntro);
        }

        if (!notFromIntro)
        {
            if (currentIntroIndex < introData.Length)
            {
                StartCoroutine(IntroTextRoutine(introData[currentIntroIndex]));
            }
            else
            {
                EventManager.Instance.StartGame();
            }
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