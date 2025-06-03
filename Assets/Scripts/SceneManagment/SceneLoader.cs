using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    [SerializeField] private Animator fadeAnimator;

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Instance.FadeOut();
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(int scene, float delay)
    {
        StartCoroutine(LoadSceneRoutine(scene, delay));
    }

    IEnumerator LoadSceneRoutine(int scene, float delay)
    {
        fadeAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    private void FadeOut()
    {
        fadeAnimator.SetTrigger("FadeOut");
    }

    public void Cancel()
    {
        fadeAnimator.SetTrigger("Cancel");
    }
}