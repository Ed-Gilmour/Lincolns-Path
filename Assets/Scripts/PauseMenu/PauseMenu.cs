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
        GameData gameData = SaveSystem.LoadData<GameData>(SaveSystem.DataFilePath(SaveSystem.GameDataFileName));
        volumeSlider.value = gameData.volume;
        accessibilityToggle.isOn = gameData.isAccessibilityFont;

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
        GameData gameData = SaveSystem.LoadData<GameData>(SaveSystem.DataFilePath(SaveSystem.GameDataFileName));
        gameData.volume = value;
        SaveSystem.SaveData(gameData, SaveSystem.DataFilePath(SaveSystem.GameDataFileName));
    }

    public void SetAccessibilityFont(bool isOn)
    {
        isAccessibilityFont = isOn;
        GameData gameData = SaveSystem.LoadData<GameData>(SaveSystem.DataFilePath(SaveSystem.GameDataFileName));
        gameData.isAccessibilityFont = isOn;
        SaveSystem.SaveData(gameData, SaveSystem.DataFilePath(SaveSystem.GameDataFileName));
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