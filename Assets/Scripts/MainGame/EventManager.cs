using System.Collections;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    [SerializeField] private TMP_FontAsset writtenFont;
    [SerializeField] private TMP_FontAsset typedFont;
    [SerializeField] private Animator dateAnimator;
    [SerializeField] private TypewriterEffect dateTypewriterEffect;
    [SerializeField] private Animator decisionsAnimator;
    [SerializeField] private TextMeshProUGUI decision2Text;
    [SerializeField] private TextMeshProUGUI decision1Text;
    [SerializeField] private AudioManager.AudioClipData personClickSound;
    [SerializeField] private AudioManager.AudioClipData personTalkSound;
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
    [SerializeField] private float gameStartDelay;
    [SerializeField] private float eventDelay;
    [SerializeField] private ButtonSounds[] decisionSounds;
    [SerializeField] private SelectableAnimator[] decisionAnimators;
    [SerializeField] private GameObject[] objectsToActivateOnStart;
    [SerializeField] private EventData[] events;
    private EventData currentEvent;
    private int currentEventIndex;
    private const float writtenLineSpacing = -30f;
    private const float typedLineSpacing = -5f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateAccessibilityFont();
    }

    private void UpdateAccessibilityFont()
    {
        if (PauseMenu.Instance.GetIsAccessibilityFont())
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
        StartCoroutine(PlayEventRoutine(gameStartDelay));
    }

    private IEnumerator PlayEventRoutine(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

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
            personTitleText.text = currentEvent.personTitle;
        }

        eventAnimator.SetTrigger("Open");
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo stateInfo = eventAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Open") && stateInfo.normalizedTime >= 1f;
        });

        if (!isLetter)
        {
            personTalkSound.Play();
            personCanvasAnimator.SetTrigger("Show");
        }

        dateTypewriterEffect.PlayTypewriterEffect(currentEvent.date);

        decisionsAnimator.SetTrigger("Show");
        dateAnimator.SetTrigger("Show");
    }

    public void CloseEvent(bool isDecision1)
    {
        StartCoroutine(CloseEventRoutine(isDecision1));
        if (currentEventIndex < events.Length - 1)
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

            StartCoroutine(PlayEventRoutine(eventDelay));
        }
    }

    private IEnumerator CloseEventRoutine(bool isDecision1)
    {
        bool isLetter = currentEvent.eventType == GameEventType.Letter;

        if (!isLetter)
        {
            personCanvasAnimator.SetTrigger("Hide");
        }

        decisionsAnimator.SetTrigger("Hide");
        dateAnimator.SetTrigger("Hide");

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