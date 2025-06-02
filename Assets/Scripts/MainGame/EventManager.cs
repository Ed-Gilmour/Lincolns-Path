using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    [SerializeField] private TMP_FontAsset writtenFont;
    [SerializeField] private TMP_FontAsset typedFont;
    [SerializeField] private GameObject blackBackground;
    [SerializeField] private AudioSource fireAudio;
    [SerializeField] private Animator backgroundAnimator;
    [SerializeField] private Animator statsAnimator;
    [SerializeField] private Animator dateAnimator;
    [SerializeField] private TypewriterEffect dateTypewriterEffect;
    [SerializeField] private Animator decisionsAnimator;
    [SerializeField] private TextMeshProUGUI decision2Text;
    [SerializeField] private TextMeshProUGUI decision1Text;
    [SerializeField] private AudioManager.AudioClipData personClickSound;
    [SerializeField] private SpriteRenderer personSpriteRenderer;
    [SerializeField] private Animator personAnimator;
    [SerializeField] private Animator personMoveAnimator;
    [SerializeField] private Animator personCanvasAnimator;
    [SerializeField] private TextMeshProUGUI personTitleText;
    [SerializeField] private TextMeshProUGUI personText;
    [SerializeField] private AudioManager.AudioClipData letterClickSound;
    [SerializeField] private Animator letterAnimator;
    [SerializeField] private Animator letterMoveAnimator;
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private float eventDelay;
    [SerializeField] private float lossDelay;
    [SerializeField] private EventData militaryLowEvent;
    [SerializeField] private EventData militaryHighEvent;
    [SerializeField] private EventData moneyLowEvent;
    [SerializeField] private EventData moneyHighEvent;
    [SerializeField] private EventData northLowEvent;
    [SerializeField] private EventData northHighEvent;
    [SerializeField] private EventData southLowEvent;
    [SerializeField] private EventData southHighEvent;
    [SerializeField] private ButtonSounds[] decisionSounds;
    [SerializeField] private SelectableAnimator[] decisionAnimators;
    [SerializeField] private GameObject[] objectsToActivateOnStart;
    [SerializeField] private EventData[] events;
    private EventData currentEvent;
    private EventData lossEvent;
    private int currentEventIndex;
    private List<bool> allDecisions = new();
    private const float writtenLineSpacing = -30f;
    private const float typedLineSpacing = -5f;

    private void Awake()
    {
        Instance = this;
        StatManager.Instance.onStatsChanged += OnStatsChanged;
        UpdateAccessibilityFont(PauseMenu.Instance.GetIsAccessibilityFont());
        PauseMenu.Instance.OnAccessibilityFontChanged += UpdateAccessibilityFont;
        IntroManager.Instance.OnFinishedSwipeOut += DisplayTimeline;
    }

    private void DisplayTimeline(bool notFromIntro)
    {
        if (notFromIntro)
        {
            TimelineManager.Instance.DisplayTimeline(events, allDecisions);
        }
    }

    private void OnStatsChanged(StatManager.StatSet statSet)
    {
        EventData lossEventData = GetEventDataForLoss(statSet);
        if (lossEventData != null)
        {
            lossEvent = lossEventData;
        }
    }

    private EventData GetEventDataForLoss(StatManager.StatSet statSet)
    {
        EventData lossEventData = null;

        if (statSet.military <= 0f)
            lossEventData = militaryLowEvent;
        else if (statSet.military >= StatManager.MaxStat)
            lossEventData = militaryHighEvent;
        else if (statSet.money <= 0)
            lossEventData = moneyLowEvent;
        else if (statSet.money >= StatManager.MaxStat)
            lossEventData = moneyHighEvent;
        else if (statSet.north <= 0)
            lossEventData = northLowEvent;
        else if (statSet.north >= StatManager.MaxStat)
            lossEventData = northHighEvent;
        else if (statSet.south <= 0)
            lossEventData = southLowEvent;
        else if (statSet.south >= StatManager.MaxStat)
            lossEventData = southHighEvent;

        return lossEventData;
    }

    public EventData GetLossEvent()
    {
        return lossEvent;
    }

    private void UpdateAccessibilityFont(bool isOn)
    {
        if (isOn)
        {
            letterText.font = typedFont;
            letterText.lineSpacing = typedLineSpacing;
        }
        else
        {
            letterText.font = writtenFont;
            letterText.lineSpacing = writtenLineSpacing;
        }
    }

    public void StartGame()
    {
        foreach (GameObject obj in objectsToActivateOnStart)
        {
            obj.SetActive(true);
        }
        currentEvent = events[currentEventIndex];
        StartCoroutine(PlayEventRoutine());
    }

    private IEnumerator PlayEventRoutine()
    {
        yield return new WaitForSeconds(eventDelay + currentEvent.additionalDelay);

        if (currentEvent.eventType == GameEventType.CutToBlack)
        {
            if (currentEvent.lincolnEventType == LincolnEventType.LossEvent)
            {
                blackBackground.SetActive(true);
                statsAnimator.gameObject.SetActive(false);
                fireAudio.mute = true;
                backgroundAnimator.SetTrigger("Hide");
            }

            currentEvent.showSound.Play();

            yield return new WaitForSeconds(currentEvent.screenTextDelay);

            IntroManager.IntroTextData introTextData = new(null, currentEvent.eventDescription, currentEvent.continueTextTime);
            IntroManager.Instance.StartCoroutine(IntroManager.Instance.IntroTextRoutine(introTextData));

            yield break;
        }

        StatManager.Instance.SetCurrentEvent(currentEvent);
        bool isLetter = currentEvent.eventType == GameEventType.Letter;
        Animator eventAnimator = isLetter ? letterAnimator : personAnimator;

        foreach (SelectableAnimator selectableAnimator in decisionAnimators)
        {
            selectableAnimator.SetTargetAnimator(isLetter ? letterMoveAnimator : personMoveAnimator);
        }
        foreach (ButtonSounds buttonSounds in decisionSounds)
        {
            buttonSounds.SetClickSound(isLetter ? letterClickSound : personClickSound);
        }

        eventAnimator.gameObject.SetActive(true);
        (isLetter ? letterText : personText).text = currentEvent.eventDescription;
        decision1Text.text = currentEvent.decision1Description;
        decision2Text.text = currentEvent.decision2Description;

        if (!isLetter)
        {
            personSpriteRenderer.sprite = currentEvent.personSprite;
            personTitleText.text = currentEvent.title;
        }

        eventAnimator.SetTrigger("Open");
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = eventAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Open") && stateInfo.normalizedTime >= 1f;
        });

        if (!isLetter)
        {
            currentEvent.showSound.Play();
            personCanvasAnimator.SetTrigger("Show");
        }

        if (currentEvent.date.Length > 0)
        {
            dateTypewriterEffect.PlayTypewriterEffect(currentEvent.date);
            dateAnimator.SetTrigger("Show");
        }

        decisionsAnimator.SetTrigger("Show");
    }

    public void CloseEvent(bool isDecision1)
    {
        allDecisions.Add(isDecision1);
        StartCoroutine(CloseEventRoutine(isDecision1, currentEvent));
        if (currentEvent.lincolnEventType != LincolnEventType.LossEvent)
        {
            if (currentEventIndex < events.Length - 1 || lossEvent != null)
            {
                if (lossEvent == null)
                {
                    if (isDecision1 && currentEvent.decision1FollowingEvent != null)
                    {
                        currentEvent = currentEvent.decision1FollowingEvent;
                    }
                    else if (!isDecision1 && currentEvent.decision2FollowingEvent != null)
                    {
                        currentEvent = currentEvent.decision2FollowingEvent;
                    }
                    else
                    {
                        currentEventIndex++;
                        currentEvent = events[currentEventIndex];
                    }
                }
                else
                {
                    currentEvent = lossEvent;
                }

                StartCoroutine(PlayEventRoutine());
            }
        }
        else
        {
            StartCoroutine(LossRoutine());
        }
    }

    private IEnumerator LossRoutine()
    {
        yield return new WaitForSeconds(lossDelay);
        backgroundAnimator.SetTrigger("Hide");
        statsAnimator.SetTrigger("Hide");
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = backgroundAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("MainGameFadeOut") && stateInfo.normalizedTime >= 1f;
        });
        DisplayTimeline(true);
    }

    private IEnumerator CloseEventRoutine(bool isDecision1, EventData closingEvent)
    {
        bool isLetter = closingEvent.eventType == GameEventType.Letter;

        if (!isLetter)
        {
            personCanvasAnimator.SetTrigger("Hide");
        }

        decisionsAnimator.SetTrigger("Hide");

        if (closingEvent.date.Length > 0)
        {
            dateAnimator.SetTrigger("Hide");
        }

        Animator eventAnimator = isLetter ? letterAnimator : personAnimator;

        eventAnimator.SetTrigger(isDecision1 ? "Left" : "Right");
        AnimatorStateInfo stateInfo = eventAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = eventAnimator.GetCurrentAnimatorStateInfo(0);
            return (stateInfo.IsName("AwayLeft") || stateInfo.IsName("AwayRight")) && stateInfo.normalizedTime >= 1f;
        });
        eventAnimator.gameObject.SetActive(false);
    }
}