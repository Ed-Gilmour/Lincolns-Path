using UnityEngine;

public class PitchRandomizer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float pitchVariation;

    public void RandomizePitch()
    {
        audioSource.pitch = Random.Range(1f - pitchVariation, 1f + pitchVariation);
    }
}