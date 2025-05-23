using System.Collections;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private GameObject personObject;
    [SerializeField] private TextMeshProUGUI personText;
    [SerializeField] private GameObject letterObject;
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private float startGameDelay;
    [SerializeField] private GameObject[] objectsToActivateOnStart;
    [SerializeField] private EventDataScriptableObject[] events;
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
        PlayEvent();
    }

    private void PlayEvent()
    {
        EventDataScriptableObject eventData = events[currentEventIndex];
        if (eventData.eventType == GameEventType.Letter)
        {
            SetUpEvent(letterObject, letterText, eventData.eventDescription);
        }
        else
        {
            SetUpEvent(personObject, personText, eventData.eventDescription);
        }
        currentEventIndex++;
    }

    private void SetUpEvent(GameObject eventObj, TextMeshProUGUI eventText, string eventDescription)
    {
        eventObj.SetActive(true);
        eventText.text = eventDescription;
    }
}