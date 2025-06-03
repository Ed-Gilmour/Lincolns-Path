using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public GameObject optionsMenu;
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle accessibilityToggle;
    [SerializeField] private GameObject[] mainGameObjects;
    [HideInInspector] public bool restarted;
    private bool isPauseMenuOpen;
    private bool isAccessibilityFont;
    public event Action<bool> OnAccessibilityFontChanged;

    private void Awake()
    {
        Singleton();
        InitializeUI();
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene current, Scene next)
    {
        foreach (GameObject obj in mainGameObjects)
        {
            if (this == null || obj == null) continue;

            obj.SetActive(SceneManager.GetActiveScene().buildIndex == 1);
        }
    }

    private void InitializeUI()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        accessibilityToggle.isOn = PlayerPrefs.GetString("AccessibilityFont", false.ToString()) == true.ToString();

        volumeSlider.onValueChanged.AddListener(SetVolume);
        accessibilityToggle.onValueChanged.AddListener(SetAccessibilityFont);
    }

    private void Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Instance.restarted = false;
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetVolume(float value)
    {
        AudioManager.SetVolume(mainMixer, value);
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }

    public void SetAccessibilityFont(bool isOn)
    {
        isAccessibilityFont = isOn;
        PlayerPrefs.SetString("AccessibilityFont", isAccessibilityFont.ToString());
        PlayerPrefs.Save();
        OnAccessibilityFontChanged?.Invoke(isOn);
    }

    public bool GetIsAccessibilityFont()
    {
        return isAccessibilityFont;
    }

    public void SetPauseMenuOpen(bool isOpen)
    {
        isPauseMenuOpen = isOpen;
    }

    public bool GetIsPauseMenuOpen()
    {
        return isPauseMenuOpen;
    }
}