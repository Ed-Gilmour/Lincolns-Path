using UnityEngine;
using TMPro;

public class TimelineEventToggle : MonoBehaviour
{
    [SerializeField] private GameObject lincolnIcon;
    [SerializeField] private TextMeshProUGUI dateText;

    public void SetTimelineToggle(bool isLincoln, string date)
    {
        lincolnIcon.SetActive(isLincoln);
        dateText.text = date;
    }
}