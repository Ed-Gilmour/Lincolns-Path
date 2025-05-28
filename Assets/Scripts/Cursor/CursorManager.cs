using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }
    public Texture2D defaultCursor;
    public Texture2D selectableCursor;
    [HideInInspector] public bool isOverSelectable;
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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}