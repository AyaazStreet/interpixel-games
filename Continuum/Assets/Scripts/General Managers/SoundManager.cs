using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioMixerGroup mixer_SFX;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        PlaySoundLoop(SoundManager.Sound.loop_factory);
    }

    public enum Sound
    {
        snd_slowTime,
        snd_accelTime,
        snd_stopTime,
        snd_throw,
        snd_interact_btn,
        snd_interact_switch,
        snd_doorOpen,
        snd_footstep,
        loop_factory,
        snd_splat,
        snd_fall,
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

    private static GameObject loopPlayer;
    private static AudioSource loopAudioSource;

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

        audioSource.outputAudioMixerGroup = Instance.mixer_SFX;
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
        oneShotAudioSource.outputAudioMixerGroup = Instance.mixer_SFX;
        oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static void PlaySound(Sound sound, float delay)
    {
        GameObject soundGameObject = new GameObject("DelayedSoundPlayer");
        //soundGameObject.transform.position = positon;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);

        audioSource.outputAudioMixerGroup = Instance.mixer_SFX;
        audioSource.PlayDelayed(delay);

        Destroy(soundGameObject, delay + audioSource.clip.length + 0.1f);
    }

    public static void PlaySoundLoop(Sound sound)
    {
        if (loopPlayer == null)
        {
            loopPlayer = new GameObject("SoundPlayer");
            loopAudioSource = loopPlayer.AddComponent<AudioSource>();
        }
        loopAudioSource.clip = GetAudioClip(sound);
        loopAudioSource.loop = true;
        loopAudioSource.volume = 0.5f;

        loopAudioSource.outputAudioMixerGroup = Instance.mixer_SFX;
        loopAudioSource.Play();
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
