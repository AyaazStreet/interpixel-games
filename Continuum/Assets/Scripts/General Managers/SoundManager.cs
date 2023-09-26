using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioMixerGroup grp_master;
    public AudioMixerGroup grp_spatial;
    public AudioMixerGroup grp_nonspatial;
    public AudioMixerGroup grp_background;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SoundManager.Ininitalize();
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
        snd_footstep_wet,
        loop_factory,
        snd_splat,
        snd_fall,
        snd_drip,
        snd_click,
        snd_hover
    }

    private static Dictionary<Sound, float> soundTimerDictionary;
    public static void Ininitalize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        foreach (Sound s in Enum.GetValues(typeof(Sound)))
        {
            soundTimerDictionary[s] = 0f;
        }
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

    private static bool CanPlaySound(Sound sound)
    {
        if (soundTimerDictionary.ContainsKey(sound))
        {
            float lastTimePlayed = soundTimerDictionary[sound];
            float timerMax = 0.03f;

            if (lastTimePlayed + timerMax < Time.time)
            {
                soundTimerDictionary[sound] = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public static void PlaySound(Sound sound, Vector3 positon)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("SpatialSoundPlayer");
            soundGameObject.transform.position = positon;

            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.maxDistance = 15f;
            audioSource.minDistance = 8f;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.dopplerLevel = 0f;

            audioSource.outputAudioMixerGroup = Instance.grp_spatial;
            audioSource.Play();

            Destroy(soundGameObject, audioSource.clip.length + 0.1f);
        }
    }

    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (oneShotPlayer == null)
            {
                oneShotPlayer = new GameObject("SoundPlayer");
                oneShotAudioSource = oneShotPlayer.AddComponent<AudioSource>();
            }
            oneShotAudioSource.outputAudioMixerGroup = Instance.grp_nonspatial;
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void PlaySound(Sound sound, float delay)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("DelayedSoundPlayer");
            //soundGameObject.transform.position = positon;

            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);

            audioSource.outputAudioMixerGroup = Instance.grp_nonspatial;
            audioSource.PlayDelayed(delay);

            Destroy(soundGameObject, delay + audioSource.clip.length + 0.1f);
        }
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

        loopAudioSource.outputAudioMixerGroup = Instance.grp_background;
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
