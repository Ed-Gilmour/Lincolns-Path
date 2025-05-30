using System;
using UnityEngine;
using UnityEngine.Audio;

using Random = UnityEngine.Random;

public static class AudioManager
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public float volume;
        public float pitchVariation;

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
        audioSource.pitch = Random.Range(1f - clipData.pitchVariation, 1f + clipData.pitchVariation);
        audioSource.clip = clipData.clip;
        audioSource.Play();
        UnityEngine.Object.Destroy(audioSource.gameObject, clipData.clip.length);
    }

    public static void SetVolume(AudioMixer mixer, float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Approximately(value, 0f) ? -80f : Mathf.Log10(value) * 20f);
    }
}