using UnityEngine;

[CreateAssetMenu(fileName = "NewEventData", menuName = "Scriptable Objects/EventData")]
public class EventData : ScriptableObject
{
    public GameEventType eventType;
    [TextArea] public string eventDescription;
    [TextArea] public string decision1Description;
    public StatManager.StatSet decision1StatsChange;
    [TextArea] public string decision2Description;
    public StatManager.StatSet decision2StatsChange;
}

public enum GameEventType { Person, Letter }