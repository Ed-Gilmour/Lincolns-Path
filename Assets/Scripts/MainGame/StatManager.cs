using System;
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
    [HideInInspector] public StatSet stats = new();
    private EventData currentEventData;
    private bool currentReverse;
    private readonly Vector2 signalSmallSize = new(8f, 8f);
    private readonly Vector2 signalLargeSize = new(12f, 12f);
    private const int minChangeForLarge = 25;
    public const int MaxStat = 100;
    public Action<StatSet> onStatsChanged;

    [Serializable]
    public class StatSet
    {
        public int military;
        public int money;
        public int north;
        public int south;

        public StatSet(int military = MaxStat / 2, int money = MaxStat / 2, int north = MaxStat / 2, int south = MaxStat / 2)
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

    public void SetCurrentEvent(EventData newEvent, bool reverse)
    {
        currentReverse = reverse;
        currentEventData = newEvent;
    }

    public void UpdateStats(bool isDecision1)
    {
        if (currentReverse) isDecision1 = !isDecision1;

        StatSet statChange = isDecision1 ? currentEventData.decision1StatsChange : currentEventData.decision2StatsChange;
        stats += statChange;
        onStatsChanged?.Invoke(stats);
        militaryFillEffect.PlayFillAnimation((float)statChange.military / MaxStat);
        moneyFillEffect.PlayFillAnimation((float)statChange.money / MaxStat);
        northFillEffect.PlayFillAnimation((float)statChange.north / MaxStat);
        southFillEffect.PlayFillAnimation((float)statChange.south / MaxStat);
    }

    public void SetStatSignals(bool isDecision1)
    {
        if (currentReverse) isDecision1 = !isDecision1;

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