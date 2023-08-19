using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public enum Sound
    {
        snd_slowTime,
        snd_accelTime,
        snd_stopTime,
        snd_throw,
        snd_interact,
        snd_doorOpen,
    }

    [Serializable]
    public struct SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }

    public SoundAudioClip[] soundAudioClipArray;

    private static GameObject oneShotPlayer;
    private static AudioSource oneShotAudioSource;

    public static void PlaySound(Sound sound, Vector3 positon)
    {
        GameObject soundGameObject = new GameObject("SpatialSoundPlayer");
        soundGameObject.transform.position = positon;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;

        audioSource.Play();

        Destroy(soundGameObject, audioSource.clip.length + 0.1f);
    }

    public static void PlaySound(Sound sound)
    {
        if (oneShotPlayer == null)
        {
            oneShotPlayer = new GameObject("SoundPlayer");
            oneShotAudioSource = oneShotPlayer.AddComponent<AudioSource>();
        }
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(SoundAudioClip clip in Instance.soundAudioClipArray)
        {
            if (clip.sound == sound)
            {
                return clip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found.");
        return null;
    }
}
