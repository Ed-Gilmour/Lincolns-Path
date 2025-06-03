using UnityEngine;
using TMPro;

public class TimelineEventToggle : MonoBehaviour
{
    [SerializeField] private GameObject lincolnIcon;
    [SerializeField] private GameObject differentIcon;
    [SerializeField] private GameObject shouldHaveIcon;
    [SerializeField] private GameObject notRealIcon;
    [SerializeField] private TextMeshProUGUI dateText;

    public void SetTimelineToggle(bool lincoln, bool shouldHave, bool notReal, string date)
    {
        lincolnIcon.SetActive(lincoln);
        shouldHaveIcon.SetActive(shouldHave);
        notRealIcon.SetActive(notReal);

        differentIcon.SetActive(!lincoln && !notReal && !shouldHave);

        dateText.text = date;
    }
}