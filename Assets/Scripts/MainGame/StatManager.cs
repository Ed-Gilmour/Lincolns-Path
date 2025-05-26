using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }
    [SerializeField] private ImageFillEffect militaryFillEffect;
    [SerializeField] private ImageFillEffect moneyFillEffect;
    [SerializeField] private ImageFillEffect northFillEffect;
    [SerializeField] private ImageFillEffect southFillEffect;
    [SerializeField] private RectTransform militarySignalRectTransform;
    [SerializeField] private RectTransform moneySignalRectTransform;
    [SerializeField] private RectTransform northSignalRectTransform;
    [SerializeField] private RectTransform southSignalRectTransform;
    private StatSet stats = new();
    private EventData currentEventData;
    private readonly Vector2 signalSmallSize = new(8f, 8f);
    private readonly Vector2 signalLargeSize = new(12f, 12f);
    private const int minChangeForLarge = 25;
    private const int maxStat = 100;

    [System.Serializable]
    public class StatSet
    {
        public int military;
        public int money;
        public int north;
        public int south;

        public StatSet(int military = maxStat / 2, int money = maxStat / 2, int north = maxStat / 2, int south = maxStat / 2)
        {
            this.military = military;
            this.money = money;
            this.north = north;
            this.south = south;
        }

        public static StatSet operator +(StatSet a, StatSet b)
        {
            return new StatSet(a.military + b.military, a.money + b.money, a.north + b.north, a.south + b.south);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SetCurrentEvent(EventData newEvent)
    {
        currentEventData = newEvent;
    }

    public void UpdateStats(bool isDecision1)
    {
        StatSet statChange = isDecision1 ? currentEventData.decision1StatsChange : currentEventData.decision2StatsChange;
        stats += statChange;
        militaryFillEffect.PlayFillAnimation((float)statChange.military / maxStat);
        moneyFillEffect.PlayFillAnimation((float)statChange.money / maxStat);
        northFillEffect.PlayFillAnimation((float)statChange.north / maxStat);
        southFillEffect.PlayFillAnimation((float)statChange.south / maxStat);
    }

    public void SetStatSignals(bool isDecision1)
    {
        StatSet statChange = isDecision1 ? currentEventData.decision1StatsChange : currentEventData.decision2StatsChange;
        SetSingleStatSignal(militarySignalRectTransform, statChange.military);
        SetSingleStatSignal(moneySignalRectTransform, statChange.money);
        SetSingleStatSignal(northSignalRectTransform, statChange.north);
        SetSingleStatSignal(southSignalRectTransform, statChange.south);
    }

    public void ClearStatSignals()
    {
        militarySignalRectTransform.gameObject.SetActive(false);
        moneySignalRectTransform.gameObject.SetActive(false);
        northSignalRectTransform.gameObject.SetActive(false);
        southSignalRectTransform.gameObject.SetActive(false);
    }

    private void SetSingleStatSignal(RectTransform rectTransform, int stat)
    {
        rectTransform.sizeDelta = Mathf.Abs(stat) >= minChangeForLarge ? signalLargeSize : signalSmallSize;
        rectTransform.gameObject.SetActive(Mathf.Abs(stat) > 0);
    }
}