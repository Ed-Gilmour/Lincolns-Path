using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCursorSelectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CursorManager cursorManager;

    private void Start()
    {
        cursorManager = CursorManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorManager.selectableCursor, cursorManager.cursorHotspot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorManager.defaultCursor, cursorManager.cursorHotspot, CursorMode.Auto);
    }
}