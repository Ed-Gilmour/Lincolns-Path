using System;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public float volume;

        public void Play()
        {
            if (clip != null)
            {
                PlayAudioClip(this);
            }
        }
    }

    public static void PlayAudioClip(AudioClipData clipData)
    {
        AudioSource audioSource = new GameObject(clipData.clip.name).AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = clipData.mixerGroup;
        audioSource.volume = clipData.volume;
        audioSource.clip = clipData.clip;
        audioSource.Play();
        UnityEngine.Object.Destroy(audioSource.gameObject, clipData.clip.length);
    }
}