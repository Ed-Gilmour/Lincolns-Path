using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCursorSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CursorManager cursorManager;
    private bool cursorIsOver;

    private void Start()
    {
        cursorManager = CursorManager.Instance;
    }

    private void OnDisable()
    {
        if (cursorIsOver)
        {
            SetCursor(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetCursor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetCursor(false);
    }

    void SetCursor(bool isSelectable)
    {
        CursorManager.Instance.isOverSelectable = isSelectable;
        cursorIsOver = isSelectable;
        Cursor.SetCursor(isSelectable ? cursorManager.selectableCursor : cursorManager.defaultCursor, cursorManager.cursorHotspot, CursorMode.Auto);
    }
}