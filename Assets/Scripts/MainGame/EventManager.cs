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
    [SerializeField] private GameObject letterObject;
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private float startGameDelay;
    [SerializeField] private GameObject[] objectsToActivateOnStart;
    [SerializeField] private EventData[] events;
    private int currentEventIndex;
    private const float letterEventTime = 3.5f;
    private const float personEventTime = 0f;

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
            SetUpEvent(letterObject, letterText, eventData);
            yield return new WaitForSeconds(letterEventTime);
        }
        else
        {
            SetUpEvent(personObject, personText, eventData);
            yield return new WaitForSeconds(personEventTime);
        }
        decisionsAnimator.SetTrigger("Show");
    }

    private void SetUpEvent(GameObject eventObj, TextMeshProUGUI eventText, EventData eventData)
    {
        eventObj.SetActive(true);
        eventText.text = eventData.eventDescription;
        decision1Text.text = eventData.decision1Description;
        decision2Text.text = eventData.decision2Description;
    }
}