using UnityEngine;

public class ButtonSceneLoader : MonoBehaviour
{
    [SerializeField] private float delay;

    public void LoadScene(int scene)
    {
        SceneLoader.Instance.LoadScene(scene, delay);
    }
}