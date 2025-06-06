using UnityEngine;
using TMPro;

public class TimelineEventToggle : MonoBehaviour
{
    [SerializeField] private GameObject lincolnIcon;
    [SerializeField] private GameObject differentIcon;
    [SerializeField] private GameObject shouldHaveIcon;
    [SerializeField] private GameObject notRealIcon;
    [SerializeField] private TextMeshProUGUI dateText;

    public void SetTimelineToggle(bool lincoln, bool shouldHave, bool notReal, string date, bool none = false)
    {
        if (!none)
        {
            lincolnIcon.SetActive(lincoln && !shouldHave);
            differentIcon.SetActive(!lincoln && !notReal && !shouldHave);
            shouldHaveIcon.SetActive(shouldHave);
            notRealIcon.SetActive(notReal);
        }

        dateText.text = date;
    }
}