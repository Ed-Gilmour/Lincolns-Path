using UnityEngine;

[CreateAssetMenu(fileName = "NewEventData", menuName = "Scriptable Objects/EventData")]
public class EventData : ScriptableObject
{
    public GameEventType eventType;
    public string personTitle;
    public Sprite personSprite;
    public AudioManager.AudioClipData personSound;
    public string date;
    [TextArea] public string eventDescription;
    [TextArea] public string decision1Description;
    public StatManager.StatSet decision1StatsChange;
    public EventData decision1FollowingEvent;
    [TextArea] public string decision2Description;
    public StatManager.StatSet decision2StatsChange;
    public EventData decision2FollowingEvent;
    public LincolnEventType lincolnEventType;
}

public enum LincolnEventType { Decision1, Decision2, Neither }

public enum GameEventType { Person, Letter }