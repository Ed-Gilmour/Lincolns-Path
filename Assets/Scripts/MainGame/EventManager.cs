using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    [SerializeField] private Animator tutorialAnimator;
    [SerializeField] private GameObject benny;
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
    [SerializeField] private Animator documentAnimator;
    [SerializeField] private Animator documentMoveAnimator;
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
    [SerializeField] private int startIndex;
    private EventData currentEvent;
    private EventData lossEvent;
    private int currentEventIndex;
    private List<bool> allDecisions = new();
    private List<(EventData, int)> futureEvents = new();
    private bool isFutureEvent;
    private const float writtenLineSpacing = -30f;
    private const float typedLineSpacing = -20f;

    private void Awake()
    {
        Instance = this;
        currentEventIndex = startIndex;
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
        bool hasLost =
        statSet.military <= 0f ||
        statSet.military >= StatManager.MaxStat ||
        statSet.money <= 0 ||
        statSet.money >= StatManager.MaxStat ||
        statSet.north <= 0 ||
        statSet.north >= StatManager.MaxStat ||
        statSet.south <= 0 ||
        statSet.south >= StatManager.MaxStat;

        while (hasLost && lossEventData == null)
        {
            int rng = Random.Range(0, 8);

            if (statSet.military <= 0f && rng == 0)
                lossEventData = militaryLowEvent;
            else if (statSet.military >= StatManager.MaxStat && rng == 1)
                lossEventData = militaryHighEvent;
            else if (statSet.money <= 0 && rng == 2)
                lossEventData = moneyLowEvent;
            else if (statSet.money >= StatManager.MaxStat && rng == 3)
                lossEventData = moneyHighEvent;
            else if (statSet.north <= 0 && rng == 4)
                lossEventData = northLowEvent;
            else if (statSet.north >= StatManager.MaxStat && rng == 5)
                lossEventData = northHighEvent;
            else if (statSet.south <= 0 && rng == 6)
                lossEventData = southLowEvent;
            else if (statSet.south >= StatManager.MaxStat && rng == 7)
                lossEventData = southHighEvent;
        }

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
        StartCoroutine(StartGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        foreach (GameObject obj in objectsToActivateOnStart)
        {
            obj.SetActive(true);
        }
        if (PauseMenu.Instance.restarted)
        {
            yield return new WaitForSeconds(1f);
            currentEvent = events[currentEventIndex];
            StartCoroutine(PlayEventRoutine());
        }
        else
        {
            yield return new WaitForSeconds(2f);
            tutorialAnimator.gameObject.SetActive(true);
        }
    }

    public void HideTutorialCanvas()
    {
        StartCoroutine(HideTutorialCanvasRoutine());
    }

    IEnumerator HideTutorialCanvasRoutine()
    {
        tutorialAnimator.SetTrigger("Hide");
        yield return new WaitForSeconds(1f);
        tutorialAnimator.gameObject.SetActive(false);
        currentEvent = events[currentEventIndex];
        yield return new WaitForSeconds(1f);
        StartCoroutine(PlayEventRoutine());
    }

    private IEnumerator PlayEventRoutine()
    {
        yield return new WaitForSeconds(eventDelay + currentEvent.additionalDelay);

        if (currentEvent.eventType == GameEventType.CutToBlack)
        {
            allDecisions.Add(true);

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
        bool isDocument = currentEvent.eventType == GameEventType.Document;
        Animator eventAnimator = isLetter ? letterAnimator : (isDocument ? documentAnimator : personAnimator);

        foreach (SelectableAnimator selectableAnimator in decisionAnimators)
        {
            selectableAnimator.SetTargetAnimator(isLetter ? letterMoveAnimator : (isDocument ? documentMoveAnimator : personMoveAnimator));
        }
        foreach (ButtonSounds buttonSounds in decisionSounds)
        {
            buttonSounds.SetClickSound(isLetter || isDocument ? letterClickSound : personClickSound);
        }

        eventAnimator.gameObject.SetActive(true);
        if (currentEvent.eventDescription.Length > 0)
        {
            (isLetter ? letterText : personText).text = currentEvent.eventDescription;
        }
        decision1Text.text = currentEvent.decision1Description;
        decision2Text.text = currentEvent.decision2Description;

        if (!isLetter && !isDocument)
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

        if (!isLetter && !isDocument)
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
                    void UpdateEvent()
                    {
                        if (!isFutureEvent)
                        {
                            currentEventIndex++;
                        }
                        else
                        {
                            isFutureEvent = false;
                        }
                        EventData futureEvent = GetCurrentFutureEventData();
                        if (futureEvent != null)
                        {
                            isFutureEvent = true;
                            currentEvent = futureEvent;
                        }
                        else
                        {
                            currentEvent = events[currentEventIndex];
                        }
                    }

                    if (isDecision1 && currentEvent.decision1FollowingEvent != null)
                    {
                        if (currentEvent.eventDelayCount > 0)
                        {
                            futureEvents.Add((currentEvent.decision1FollowingEvent, currentEventIndex + currentEvent.eventDelayCount));
                            UpdateEvent();
                        }
                        else
                        {
                            currentEvent = currentEvent.decision1FollowingEvent;
                        }
                    }
                    else if (!isDecision1 && currentEvent.decision2FollowingEvent != null)
                    {
                        if (currentEvent.eventDelayCount > 0)
                        {
                            futureEvents.Add((currentEvent.decision2FollowingEvent, currentEventIndex + currentEvent.eventDelayCount));
                            UpdateEvent();
                        }
                        else
                        {
                            currentEvent = currentEvent.decision2FollowingEvent;
                        }
                    }
                    else
                    {
                        UpdateEvent();
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

    EventData GetCurrentFutureEventData()
    {
        EventData eventData = null;
        for (int i = 0; i < futureEvents.Count; i++)
        {
            if (futureEvents[i].Item2 == currentEventIndex)
            {
                eventData = futureEvents[i].Item1;
                futureEvents.RemoveAt(i);
                break;
            }
        }
        return eventData;
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
        bool isDocument = closingEvent.eventType == GameEventType.Document;

        if (!isLetter && !isDocument)
        {
            personCanvasAnimator.SetTrigger("Hide");
        }

        decisionsAnimator.SetTrigger("Hide");

        if (closingEvent.date.Length > 0)
        {
            dateAnimator.SetTrigger("Hide");
        }

        if (closingEvent.decision2Benny && !isDecision1)
        {
            benny.SetActive(true);
        }

        Animator eventAnimator = isLetter ? letterAnimator : (isDocument ? documentAnimator : personAnimator);

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