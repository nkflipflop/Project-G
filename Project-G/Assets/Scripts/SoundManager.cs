using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager {
    public enum Sound {
        PlayerMove, PlayerHit, PlayerDie, PlayerCollect,
        Pistol, AssaultRifle, AK47, SMG, Shotgun, Sniper, TurretGun,
        BeetleMove,
        MantisMove, MantisAttack
    }

    private static Dictionary<Sound, float> _soundTimerDictionary;
    private static GameObject _oneShotGameObject;
    private static AudioSource _oneShotAudioSource;
    private static float _volume = 0.7f;
    private static float _pitch = 1f;
    private static float _randomVolume = 0.7f;
    private static float _randomPitch = 1f;
    
    public static void Initialize() {
        _soundTimerDictionary = new Dictionary<Sound, float>();
        _soundTimerDictionary[Sound.PlayerMove] = 0f;       // adding a timer for player move (example)
    }

    public static void PlaySound(Sound sound) {
        if (_oneShotGameObject == null) {
            _oneShotGameObject = new GameObject("One Shot Sound");
            _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
        }
        _oneShotAudioSource.PlayOneShot(GameConfigData.Instance.Sounds[(int)sound].audioClip);
    }

    /// <summary> Plays a sound at an exact position </summary>
    public static void PlaySound(Sound sound, Vector3 position) {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GameConfigData.Instance.Sounds[(int)sound].audioClip;
        // Sound settings
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;
        // add some randomness to the sound in order to prevent repetitiveness
        audioSource.volume = _volume * (1 + Random.Range(-_randomVolume / 2f, _randomVolume / 2));
        audioSource.pitch = _pitch * (1 + Random.Range(-_randomPitch / 2f, _randomPitch / 2));

        audioSource.Play();

        Object.Destroy(soundGameObject, audioSource.clip.length);       // destroy the sound after playing
    }

    /// <summary> Plays a general sound </summary>
    private static bool CanPlaySound(Sound sound) {
        switch(sound) {
        case Sound.PlayerMove:
            if (_soundTimerDictionary.ContainsKey(sound)) {
                return CheckLastTimePlayed(sound, 0.05f);
            }
            else {
                return true;
            }
        default:
            return true;
        }
    }

    private static bool CheckLastTimePlayed(Sound sound, float timerMax) {
        float lastTimePlayed = _soundTimerDictionary[sound];
        if (lastTimePlayed + timerMax < Time.time) {
            _soundTimerDictionary[sound] = Time.time;
            return true;
        }
        else {
            return false;
        }
    }
}
