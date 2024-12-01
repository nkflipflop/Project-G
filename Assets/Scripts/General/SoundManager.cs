using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PlayerMove,
        CharacterHit,
        PlayerDie,
        PlayerCollect,
        NoBullet,
        Pistol,
        AssaultRifle,
        SMG,
        Shotgun,
        Sniper,
        Reloaded,
        SpikeTrap,
        FireTrap,
        MetalHit
    }

    private static Dictionary<Sound, float> soundTimerDictionary = new()
    {
        [Sound.PlayerMove] = 0f,
        [Sound.NoBullet] = 0f
    };
    
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;
    private static float pitch = 1f;
    private static float randomPitch = 1f;

    /// <summary> Plays a general sound </summary>
    public static void PlaySound(Sound sound)
    {
        return;
        if (CanPlaySound(sound))
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            float volume = GameConfigData.Instance.Sounds[(int)sound].volume;
            float randomVolume = volume;
            oneShotAudioSource.volume = volume * (1 + Random.Range(-randomVolume / 3f, randomVolume / 3f));
            oneShotAudioSource.PlayOneShot(GameConfigData.Instance.Sounds[(int)sound].audioClip);
        }
    }

    /// <summary> Plays a sound at an exact position </summary>
    public static void PlaySound(Sound sound, Vector3 position)
    {
        return;
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound")
            {
                transform =
                {
                    position = position
                }
            };
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GameConfigData.Instance.Sounds[(int)sound].audioClip;

            RandomizeSound(ref audioSource, sound);
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length); // destroy the sound after playing
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        if (soundTimerDictionary.ContainsKey(sound))
        {
            return CheckLastTimePlayed(sound, GameConfigData.Instance.Sounds[(int)sound].audioClip.length);
        }
        return true;
    }

    private static bool CheckLastTimePlayed(Sound sound, float timerMax)
    {
        float lastTimePlayed = soundTimerDictionary[sound];
        if (lastTimePlayed + timerMax < Time.time)
        {
            soundTimerDictionary[sound] = Time.time;
            return true;
        }
        return false;
    }

    private static void RandomizeSound(ref AudioSource audioSource, Sound sound)
    {
        // Sound settings
        audioSource.maxDistance = 4f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;

        // add some randomness to the sound in order to prevent repetitiveness
        float volume = GameConfigData.Instance.Sounds[(int)sound].volume;
        float randomVolume = volume;
        audioSource.volume = volume * (1 + Random.Range(-randomVolume / 3f, randomVolume / 3f));
        audioSource.pitch = pitch * (1 + Random.Range(-randomPitch / 3f, randomPitch / 3f));
    }
}