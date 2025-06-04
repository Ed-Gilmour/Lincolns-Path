using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private Animator timelineAnimator;
    [SerializeField] private GameObject eventTogglePrefab;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI descision1Text;
    [SerializeField] private TextMeshProUGUI descision1ChoseText;
    [SerializeField] private TextMeshProUGUI descision2Text;
    [SerializeField] private TextMeshProUGUI descision2ChoseText;
    private Toggle previousSelectedToggle;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayTimeline(EventData[] events, List<bool> decisions)
    {
        UpdateTimelineDisplay(events[0], decisions[0], true, false);
        for (int i = 0; i < decisions.Count; i++)
        {
            EventData currentEvent = i == decisions.Count - 1 ? EventManager.Instance.GetLossEvent() : events[i];
            CreateEventToggle(currentEvent, decisions[i], false, i == 0);
            while (i != decisions.Count - 2 && (currentEvent.decision1FollowingEvent != null || currentEvent.decision2FollowingEvent != null))
            {
                if (currentEvent.decision1FollowingEvent != null && currentEvent.decision1FollowingEvent.lincolnEventType != LincolnEventType.Neither)
                {
                    CreateEventToggle(currentEvent.decision1FollowingEvent, decisions[i + 1], !decisions[i]);
                    if (decisions[i])
                    {
                        decisions.RemoveAt(i + 1);
                    }
                    currentEvent = currentEvent.decision1FollowingEvent;
                }
                else if (currentEvent.decision2FollowingEvent != null && currentEvent.decision1FollowingEvent.lincolnEventType != LincolnEventType.Neither)
                {
                    CreateEventToggle(currentEvent.decision2FollowingEvent, decisions[i + 1], decisions[i]);
                    if (!decisions[i])
                    {
                        decisions.RemoveAt(i + 1);
                    }
                    currentEvent = currentEvent.decision2FollowingEvent;
                }
                else
                {
                    break;
                }
            }
        }
        timelineAnimator.gameObject.SetActive(true);
    }

    private void CreateEventToggle(EventData eventData, bool isDecision1, bool shouldHave, bool isFirst = false)
    {
        TimelineEventToggle timelineEvent = Instantiate(eventTogglePrefab, contentTransform).GetComponent<TimelineEventToggle>();
        Toggle timelineEventToggle = timelineEvent.GetComponent<Toggle>();
        timelineEventToggle.group = toggleGroup;
        bool bothChose1 = eventData.lincolnEventType == LincolnEventType.Decision1 && isDecision1;
        bool bothChose2 = eventData.lincolnEventType == LincolnEventType.Decision2 && !isDecision1;
        bool notReal = eventData.lincolnEventType == LincolnEventType.Neither || eventData.lincolnEventType == LincolnEventType.LossEvent;
        timelineEvent.SetTimelineToggle(bothChose1 || bothChose2, shouldHave, notReal, eventData.dateShort);
        timelineEventToggle.onValueChanged.AddListener((isOn) =>
        {
            if (previousSelectedToggle == timelineEventToggle) return;

            if (previousSelectedToggle != null)
            {
                previousSelectedToggle.targetGraphic.raycastTarget = true;
            }

            timelineEventToggle.targetGraphic.raycastTarget = false;
            previousSelectedToggle = timelineEventToggle;
            UpdateTimelineDisplay(eventData, isDecision1, isOn, shouldHave);
        });
        if (isFirst)
        {
            timelineEventToggle.isOn = true;
            previousSelectedToggle = timelineEventToggle;
        }
    }

    private void UpdateTimelineDisplay(EventData eventData, bool isDecision1, bool isOn, bool noDecision)
    {
        if (!isOn) return;

        titleText.text = eventData.title;
        descriptionText.text = eventData.eventDescription;
        dateText.text = eventData.date;
        descision1Text.text = eventData.decision1Description;
        descision2Text.text = eventData.decision2Description;

        if (eventData.lincolnEventType == LincolnEventType.LossEvent && eventData.eventType == GameEventType.CutToBlack)
        {
            descision1ChoseText.text = string.Empty;
            descision2ChoseText.text = string.Empty;
            return;
        }

        if (noDecision)
        {
            if (eventData.lincolnEventType == LincolnEventType.Decision1)
            {
                descision1ChoseText.text = "Lincoln chose";
                descision2ChoseText.text = "Lincoln didn't choose";
            }
            else
            {
                descision1ChoseText.text = "Lincoln didn't choose";
                descision2ChoseText.text = "Lincoln chose";
            }
            return;
        }

        if (isDecision1)
        {
            descision1ChoseText.text = "You";
            if (eventData.lincolnEventType == LincolnEventType.Decision1)
            {
                descision1ChoseText.text += " and Lincoln chose";
                descision2ChoseText.text = "Neither chose";
            }
            else if (eventData.lincolnEventType == LincolnEventType.Decision2)
            {
                descision1ChoseText.text += " chose";
                descision2ChoseText.text = "Lincoln chose";
            }
            else
            {
                descision1ChoseText.text += " chose";
                descision2ChoseText.text = "You didn't choose";
            }
        }
        else
        {
            descision2ChoseText.text = "You";
            if (eventData.lincolnEventType == LincolnEventType.Decision2)
            {
                descision2ChoseText.text += " and Lincoln chose";
                descision1ChoseText.text = "Neither chose";
            }
            else if (eventData.lincolnEventType == LincolnEventType.Decision1)
            {
                descision2ChoseText.text += " chose";
                descision1ChoseText.text = "Lincoln chose";
            }
            else
            {
                descision2ChoseText.text += " chose";
                descision1ChoseText.text = "You didn't choose";
            }
        }
    }
}