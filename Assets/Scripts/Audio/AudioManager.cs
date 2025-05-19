using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip clip;
        public float volume;

        public void Play()
        {
            PlayAudioClip(this);
        }
    }

    public static void PlayAudioClip(AudioClipData clipData)
    {
        AudioSource audioSource = new GameObject(clipData.clip.name).AddComponent<AudioSource>();
        audioSource.volume = clipData.volume;
        audioSource.clip = clipData.clip;
        audioSource.Play();
        Destroy(audioSource.gameObject, clipData.clip.length);
    }
}