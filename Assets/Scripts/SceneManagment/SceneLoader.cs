using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float delayTime;

    public void LoadScene(int scene)
    {
        StartCoroutine(LoadSceneRoutine(scene));
    }

    IEnumerator LoadSceneRoutine(int scene)
    {
        fadeAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(scene);
    }
}