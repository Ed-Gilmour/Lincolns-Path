using UnityEngine;

[CreateAssetMenu(fileName = "NewEventData", menuName = "Scriptable Objects/EventData")]
public class EventData : ScriptableObject
{
    public GameEventType eventType;
    public string title;
    public Sprite personSprite;
    public AudioManager.AudioClipData showSound;
    public string date;
    public string dateShort;
    [TextArea] public string eventDescription;
    [TextArea] public string decision1Description;
    public StatManager.StatSet decision1StatsChange;
    public EventData decision1FollowingEvent;
    [TextArea] public string decision2Description;
    public StatManager.StatSet decision2StatsChange;
    public EventData decision2FollowingEvent;
    public LincolnEventType lincolnEventType;
    public float additionalDelay;
    public float screenTextDelay;
    public float continueTextTime;
}

public enum LincolnEventType { Decision1, Decision2, Neither, LossEvent }

public enum GameEventType { Person, Letter, CutToBlack }