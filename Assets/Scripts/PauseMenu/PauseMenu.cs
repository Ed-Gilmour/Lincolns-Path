using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle accessibilityToggle;
    private bool isPauseMenuOpen;
    private bool isAccessibilityFont;

    private void Awake()
    {
        Singleton();
        InitializeUI();
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