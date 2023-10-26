using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioMixerGroup grp_master;
    public AudioMixerGroup grp_spatial;
    public AudioMixerGroup grp_nonspatial;
    public AudioMixerGroup grp_music;
    public AudioMixerGroup grp_ambience;

    public GameObject peristentSoundPlayer;
    public AudioSource peristentAudioSource;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SoundManager.Ininitalize();

        PlaySoundLoop(SoundManager.Sound.loop_factory);

        Instance.peristentSoundPlayer = new GameObject("PeristentSoundPlayer");
        peristentAudioSource = Instance.peristentSoundPlayer.AddComponent<AudioSource>();

        if (MusicManager.Instance.transform.Find("MusicPlayer") != null)
        {
            musicPlayer = MusicManager.Instance.transform.Find("MusicPlayer").gameObject;
        }
        else
        {
            musicPlayer = null;
        }

        if (musicPlayer == null)
        {
            musicPlayer = new GameObject("MusicPlayer");
            musicAudioSource = musicPlayer.AddComponent<AudioSource>();
            musicAudioSource.transform.parent = MusicManager.Instance.transform;
        }
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
        snd_hover,
        snd_shot,
        snd_hit,
        snd_secret,
        msc_music1,
        snd_type,
        snd_pickup,
        snd_error,
        snd_slowLoop,
        snd_accelLoop,
        snd_stopLoop,
        msc_musicMenu,
        msc_music2,
        msc_music3,
        msc_music4,
        msc_music5,
        snd_burn,
        snd_infuse,
        snd_canSmash
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

    private static GameObject musicPlayer;
    private static AudioSource musicAudioSource;

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

    private static bool CanPlaySoundShort(Sound sound)
    {
        if (soundTimerDictionary.ContainsKey(sound))
        {
            float lastTimePlayed = soundTimerDictionary[sound];
            float timerMax = 0.001f;

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
            soundGameObject.transform.parent = Instance.transform;
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

    public static void PlaySoundPersistent(Sound sound)
    {
        Instance.peristentAudioSource.clip = GetAudioClip(sound);

        Instance.peristentAudioSource.outputAudioMixerGroup = Instance.grp_nonspatial;
        Instance.peristentAudioSource.Play();
    }

    public static void PlaySoundPersistentRepeat(Sound sound)
    {
        if (CanPlaySoundShort(sound))
        {
            Instance.peristentAudioSource.clip = GetAudioClip(sound);

            Instance.peristentAudioSource.outputAudioMixerGroup = Instance.grp_nonspatial;
            Instance.peristentAudioSource.Play();
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
        loopAudioSource.volume = 0.3f;

        loopAudioSource.outputAudioMixerGroup = Instance.grp_ambience;
        loopAudioSource.Play();
    }

    public static AudioSource PlaySoundCutoff(Sound sound)
    {
        
        GameObject player = new GameObject("CutoffPlayer");
        AudioSource audioSource = player.AddComponent<AudioSource>();

        audioSource.clip = GetAudioClip(sound);
        //loopAudioSource.loop = true;
        //loopAudioSource.volume = 0.5f;

        audioSource.outputAudioMixerGroup = Instance.grp_nonspatial;
        audioSource.Play();

        return audioSource;
    }

    public static IEnumerator CutoffSound(AudioSource audioSource, float fadeOutDuration)
    {
        float fadeStartTime = Time.time;

        while (audioSource.volume > 0)
        {
            float timeSinceFadeStart = Time.time - fadeStartTime;

            audioSource.volume = 1.0f - (timeSinceFadeStart / fadeOutDuration);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        audioSource.volume = 0;
        audioSource.Stop();

        Destroy(audioSource.gameObject);

        yield break;
    }

    public static void PlayMusic(Sound sound)
    {
        Debug.Log("Playing Music");

        if (musicAudioSource.clip != GetAudioClip(sound))
        {
            Debug.Log("Music Change");
            
            musicAudioSource.clip = GetAudioClip(sound);
            musicAudioSource.loop = true;
            musicAudioSource.volume = 1f;

            musicAudioSource.outputAudioMixerGroup = Instance.grp_music;
            musicAudioSource.Play();
        }
        else if (!musicAudioSource.isPlaying)
        {
            musicAudioSource.Play();
        }
    }

    public static void StopMusic()
    {
        Debug.Log("Stopping Music");

        musicAudioSource.Stop();
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
