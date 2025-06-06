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
    [SerializeField] private TextMeshProUGUI decision1Text;
    [SerializeField] private TextMeshProUGUI decision1ChoseText;
    [SerializeField] private TextMeshProUGUI decision2Text;
    [SerializeField] private TextMeshProUGUI decision2ChoseText;
    private Toggle previousSelectedToggle;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayTimeline(EventData[] events, List<(bool d1, bool r)> decisions)
    {
        UpdateTimelineDisplay(events[0], decisions[0].d1, true, false, decisions[0].r);
        List<(EventData, int)> futureEvents = new();
        bool wonGame = EventManager.Instance.wonGame;
        for (int i = 0; i < decisions.Count; i++)
        {
            EventData currentEvent = i == decisions.Count - 1 && !wonGame ? EventManager.Instance.GetLossEvent() : events[i];
            EventData futureEvent = GetCurrentFutureEventData(futureEvents, i);
            if (futureEvent != null)
            {
                currentEvent = futureEvent;
            }
            CreateEventToggle(currentEvent, decisions[i].d1, false, decisions[i].r, i == 0);
            while (i != decisions.Count - 2 && (currentEvent.decision1FollowingEvent != null || currentEvent.decision2FollowingEvent != null))
            {
                if (currentEvent.decision1FollowingEvent != null)
                {
                    if (currentEvent.eventDelayCount > 0 && decisions[i].d1)
                    {
                        futureEvents.Add((currentEvent.decision1FollowingEvent, i + currentEvent.eventDelayCount));
                        break;
                    }
                    else
                    {
                        if (currentEvent.decision1FollowingEvent.lincolnEventType != LincolnEventType.Neither || (currentEvent.decision1FollowingEvent.lincolnEventType == LincolnEventType.Neither && decisions[i].d1))
                        {
                            CreateEventToggle(currentEvent.decision1FollowingEvent, decisions[i + 1].d1, !decisions[i].d1, decisions[i].r);
                        }
                        if (decisions[i].d1)
                        {
                            decisions.RemoveAt(i + 1);
                        }
                        currentEvent = currentEvent.decision1FollowingEvent;
                    }
                }
                else if (currentEvent.decision2FollowingEvent != null)
                {
                    if (currentEvent.eventDelayCount > 0 && !decisions[i].d1)
                    {
                        futureEvents.Add((currentEvent.decision2FollowingEvent, i + currentEvent.eventDelayCount));
                        break;
                    }
                    else
                    {
                        if (currentEvent.decision2FollowingEvent.lincolnEventType != LincolnEventType.Neither || (currentEvent.decision2FollowingEvent.lincolnEventType == LincolnEventType.Neither && !decisions[i].d1))
                        {
                            CreateEventToggle(currentEvent.decision2FollowingEvent, decisions[i + 1].d1, decisions[i].d1, decisions[i].r);
                        }
                        if (!decisions[i].d1)
                        {
                            decisions.RemoveAt(i + 1);
                        }
                        currentEvent = currentEvent.decision2FollowingEvent;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        if (wonGame)
        {
            CreateEventToggle(null, false, false, false, textData: StatManager.Instance.stats.south >= EventManager.SouthToNotDie ? EventManager.Instance.endGameText2Lived : EventManager.Instance.endGameText2Died);
        }
        timelineAnimator.gameObject.SetActive(true);
    }

    EventData GetCurrentFutureEventData(List<(EventData, int)> futureEvents, int currentEventIndex)
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

    private void CreateEventToggle(EventData eventData, bool isDecision1, bool shouldHave, bool reverse, bool isFirst = false, IntroManager.IntroTextData textData = null)
    {
        TimelineEventToggle timelineEvent = Instantiate(eventTogglePrefab, contentTransform).GetComponent<TimelineEventToggle>();
        Toggle timelineEventToggle = timelineEvent.GetComponent<Toggle>();
        timelineEventToggle.group = toggleGroup;
        if (eventData != null)
        {
            bool bothChose1 = eventData.lincolnEventType == LincolnEventType.Decision1 && isDecision1;
            bool bothChose2 = eventData.lincolnEventType == LincolnEventType.Decision2 && !isDecision1;
            bool notReal = eventData.lincolnEventType == LincolnEventType.Neither || eventData.lincolnEventType == LincolnEventType.LossEvent;
            timelineEvent.SetTimelineToggle(bothChose1 || bothChose2, shouldHave, notReal, eventData.dateShort);
        }
        else
        {
            bool notDie = StatManager.Instance.stats.south >= EventManager.SouthToNotDie;
            timelineEvent.SetTimelineToggle(false, false, notDie, "4/14/65", !notDie);
        }
        timelineEventToggle.onValueChanged.AddListener((isOn) =>
        {
            if (previousSelectedToggle == timelineEventToggle) return;

            if (previousSelectedToggle != null)
            {
                previousSelectedToggle.targetGraphic.raycastTarget = true;
            }

            timelineEventToggle.targetGraphic.raycastTarget = false;
            previousSelectedToggle = timelineEventToggle;
            UpdateTimelineDisplay(eventData, isDecision1, isOn, shouldHave, reverse, textData);
        });
        if (isFirst)
        {
            timelineEventToggle.isOn = true;
            previousSelectedToggle = timelineEventToggle;
        }
    }

    private void UpdateTimelineDisplay(EventData eventData, bool isDecision1, bool isOn, bool noDecision, bool reverse, IntroManager.IntroTextData textData = null)
    {
        if (!isOn) return;

        bool notDie = StatManager.Instance.stats.south >= EventManager.SouthToNotDie;
        titleText.text = eventData != null ? eventData.title : (notDie ? "Ford's Theater" : "Assassination");
        descriptionText.text = eventData != null ? eventData.eventDescription : textData.introText;
        dateText.text = eventData != null ? eventData.date : "April 14, 1865";

        TextMeshProUGUI d1Text = reverse ? decision2Text : decision1Text;
        TextMeshProUGUI d2Text = reverse ? decision1Text : decision2Text;
        TextMeshProUGUI d1ChoseText = reverse ? decision2ChoseText : decision1ChoseText;
        TextMeshProUGUI d2ChoseText = reverse ? decision1ChoseText : decision2ChoseText;

        if (eventData != null)
        {
            d1Text.text = eventData.decision1Description;
            d2Text.text = eventData.decision2Description;
        }

        if (eventData == null || (eventData.lincolnEventType == LincolnEventType.LossEvent && eventData.eventType == GameEventType.CutToBlack))
        {
            d1ChoseText.text = string.Empty;
            d2ChoseText.text = string.Empty;
            d1Text.text = string.Empty;
            d2Text.text = string.Empty;
            return;
        }

        if (noDecision)
        {
            if (eventData.lincolnEventType == LincolnEventType.Decision1)
            {
                d1ChoseText.text = "Lincoln chose";
                d2ChoseText.text = "Lincoln didn't choose";
            }
            else
            {
                d1ChoseText.text = "Lincoln didn't choose";
                d2ChoseText.text = "Lincoln chose";
            }
            return;
        }

        if (isDecision1)
        {
            d1ChoseText.text = "You";
            if (eventData.lincolnEventType == LincolnEventType.Decision1)
            {
                d1ChoseText.text += " and Lincoln chose";
                d2ChoseText.text = "Neither chose";
            }
            else if (eventData.lincolnEventType == LincolnEventType.Decision2)
            {
                d1ChoseText.text += " chose";
                d2ChoseText.text = "Lincoln chose";
            }
            else
            {
                d1ChoseText.text += " chose";
                d2ChoseText.text = "You didn't choose";
            }
        }
        else
        {
            d2ChoseText.text = "You";
            if (eventData.lincolnEventType == LincolnEventType.Decision2)
            {
                d2ChoseText.text += " and Lincoln chose";
                d1ChoseText.text = "Neither chose";
            }
            else if (eventData.lincolnEventType == LincolnEventType.Decision1)
            {
                d2ChoseText.text += " chose";
                d1ChoseText.text = "Lincoln chose";
            }
            else
            {
                d2ChoseText.text += " chose";
                d1ChoseText.text = "You didn't choose";
            }
        }
    }
}