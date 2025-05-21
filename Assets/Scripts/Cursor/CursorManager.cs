using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D selectableCursor;

    static CursorManager _cursorManager;
    public static CursorManager Instance { get { return _cursorManager; } }

    [HideInInspector] public readonly Vector2 cursorHotspot = new(11, 0);

    private void Awake()
    {
        Singleton();
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene current, Scene next)
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    private void Singleton()
    {
        if (_cursorManager != null && _cursorManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _cursorManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}