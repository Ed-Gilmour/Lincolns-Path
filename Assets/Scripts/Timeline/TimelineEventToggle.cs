using UnityEngine;
using TMPro;

public class TimelineEventToggle : MonoBehaviour
{
    [SerializeField] private GameObject lincolnIcon;
    [SerializeField] private GameObject notRealIcon;
    [SerializeField] private TextMeshProUGUI dateText;

    public void SetTimelineToggle(bool isLincoln, bool notReal, string date)
    {
        lincolnIcon.SetActive(isLincoln);
        notRealIcon.SetActive(notReal);
        dateText.text = date;
    }
}