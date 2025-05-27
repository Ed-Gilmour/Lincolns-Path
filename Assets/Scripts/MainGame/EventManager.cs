using System.Collections;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    [SerializeField] private Animator decisionsAnimator;
    [SerializeField] private TextMeshProUGUI decision2Text;
    [SerializeField] private TextMeshProUGUI decision1Text;
    [SerializeField] private GameObject personObject;
    [SerializeField] private TextMeshProUGUI personText;
    [SerializeField] private Animator letterAnimator;
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private float startGameDelay;
    [SerializeField] private GameObject[] objectsToActivateOnStart;
    [SerializeField] private EventData[] events;
    private int currentEventIndex;

    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        foreach (GameObject obj in objectsToActivateOnStart)
        {
            obj.SetActive(true);
        }
        yield return new WaitForSeconds(startGameDelay);
        StartCoroutine(PlayEventRoutine());
    }

    private IEnumerator PlayEventRoutine()
    {
        EventData eventData = events[currentEventIndex];
        StatManager.Instance.SetCurrentEvent(eventData);
        if (eventData.eventType == GameEventType.Letter)
        {
            SetUpEvent(letterAnimator.gameObject, letterText, eventData);
            letterAnimator.SetTrigger("Open");
            yield return new WaitUntil(() =>
            {
                AnimatorStateInfo stateInfo = letterAnimator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName("LetterOpen") && stateInfo.normalizedTime >= 1f;
            });
        }
        else
        {
            SetUpEvent(personObject, personText, eventData);
        }
        decisionsAnimator.SetTrigger("Show");
    }

    public void CloseEvent(bool isDecision1)
    {
        StartCoroutine(CloseEventRoutine(isDecision1));
    }

    private IEnumerator CloseEventRoutine(bool isDecision1)
    {
        EventData eventData = events[currentEventIndex];
        decisionsAnimator.SetTrigger("Hide");
        if (eventData.eventType == GameEventType.Letter)
        {
            letterAnimator.SetTrigger(isDecision1 ? "Left" : "Right");
            AnimatorStateInfo stateInfo = letterAnimator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitUntil(() =>
            {
                AnimatorStateInfo stateInfo = letterAnimator.GetCurrentAnimatorStateInfo(0);
                return (stateInfo.IsName("LetterAwayLeft") || stateInfo.IsName("LetterAwayRight")) && stateInfo.normalizedTime >= 1f;
            });
            letterAnimator.gameObject.SetActive(false);
        }
        else
        {

        }
    }

    private void SetUpEvent(GameObject eventObj, TextMeshProUGUI eventText, EventData eventData)
    {
        eventObj.SetActive(true);
        eventText.text = eventData.eventDescription;
        decision1Text.text = eventData.decision1Description;
        decision2Text.text = eventData.decision2Description;
    }
}