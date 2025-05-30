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
    [SerializeField] private GraphicFadeEffect militarySignalFadeEffect;
    [SerializeField] private GraphicFadeEffect moneySignalFadeEffect;
    [SerializeField] private GraphicFadeEffect northSignalFadeEffect;
    [SerializeField] private GraphicFadeEffect southSignalFadeEffect;
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
        SetSingleStatSignal(militarySignalRectTransform, militarySignalFadeEffect, statChange.military);
        SetSingleStatSignal(moneySignalRectTransform, moneySignalFadeEffect, statChange.money);
        SetSingleStatSignal(northSignalRectTransform, northSignalFadeEffect, statChange.north);
        SetSingleStatSignal(southSignalRectTransform, southSignalFadeEffect, statChange.south);
    }

    public void ClearStatSignals()
    {
        militarySignalFadeEffect.SetFade(false);
        moneySignalFadeEffect.SetFade(false);
        northSignalFadeEffect.SetFade(false);
        southSignalFadeEffect.SetFade(false);
    }

    private void SetSingleStatSignal(RectTransform rectTransform, GraphicFadeEffect fadeEffect, int change)
    {
        rectTransform.sizeDelta = Mathf.Abs(change) >= minChangeForLarge ? signalLargeSize : signalSmallSize;
        fadeEffect.SetFade(Mathf.Abs(change) > 0);
    }
}