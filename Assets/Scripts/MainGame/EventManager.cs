using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private GameObject mainGameBackground;
    [SerializeField] private GameObject tempEvent;
    [SerializeField] private float startGameDelay;

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        mainGameBackground.SetActive(true);
        yield return new WaitForSeconds(startGameDelay);
        tempEvent.SetActive(true);
    }
}