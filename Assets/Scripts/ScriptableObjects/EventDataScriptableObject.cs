using UnityEngine;

[CreateAssetMenu(fileName = "NewEventData", menuName = "Scriptable Objects/EventData")]
public class EventDataScriptableObject : ScriptableObject
{
    public GameEventType eventType;
    [TextArea] public string eventDescription;
    [TextArea] public string decision1Description;
    [TextArea] public string decision2Description;
}

public enum GameEventType { Person, Letter }