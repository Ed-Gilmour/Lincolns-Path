using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;
    public Texture2D selectableCursor;

    static CursorManager _cursorManager;
    public static CursorManager Instance { get { return _cursorManager; } }

    [HideInInspector] public readonly Vector2 cursorHotspot = new(9, 2);

    void Awake()
    {
        Singleton();
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    void OnSceneChange(Scene current, Scene next)
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
    }

    void Singleton()
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