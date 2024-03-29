﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioManager 
{

    public enum Music
    {
        Menu,
        Town,
        Water,
        Fire,
        Earth,
        Air,
        PlayerWon
    }

    public enum Sound
    {
        AirAttack,
        EarthAttack,
        FireAttack,
        WaterAttack,
        BerserkerDamaged,
        BerserkerDeath,
        BerserkerEffect,
        EtherealDamaged,
        EtherealDeath,
        EtherealEffect,
        GargoyleDamaged,
        GargoyleDeath,
        GargoyleEffect,
        ImpDamaged,
        ImpDeath,
        ImpEffect,
        Birds,
        Buy,
        CantBuy,
        Confirm,
        Fire,
        GetSoul,
        LevelUp,
        Potion,
        Rain,
        Wind,
        PlayerDamaged,
        PlayerDeath,
        Running,
        Walking
    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    private static AudioMixerGroup soundMixerGroup;

    public static void Initialize(AudioMixerGroup audioMixerGroup)
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.Walking] = 0f;
        soundTimerDictionary[Sound.Running] = 0f;
        soundTimerDictionary[Sound.PlayerDamaged] = 0f;
        soundMixerGroup = audioMixerGroup;
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            audioSource.spatialBlend = 1;
            audioSource.minDistance = 1;
            audioSource.maxDistance = 5;
            audioSource.volume = 0.5f;
            audioSource.outputAudioMixerGroup = soundMixerGroup;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound)
    {
        if(CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.ignoreListenerPause = true;
            audioSource.volume = 0.25f;
            audioSource.outputAudioMixerGroup = soundMixerGroup;
            audioSource.PlayOneShot(GetAudioClip(sound));
            Object.Destroy(soundGameObject, GetAudioClip(sound).length);
        }
    }
    
    public static void PlayMusic(Music music, AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.clip = GetAudioClip(music);
        audioSource.Play();
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch(sound)
        {
            case Sound.Walking:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float walkingTimerMax = 0.5f;
                    if (lastTimePlayed + walkingTimerMax < Time.time)
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
                    return false;
            case Sound.Running:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float runningTimerMax = 0.4f;
                    if (lastTimePlayed + runningTimerMax < Time.time)
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
                    return false;
            case Sound.PlayerDamaged:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float damagedTimerMax = 1f;
                    if (lastTimePlayed + damagedTimerMax < Time.time)
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
                    return false;
            default:
                return true;
        }
    }
    
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(SoundAudioClip soundAudioClip in SoundController.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
                return soundAudioClip.audioClip;
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    private static AudioClip GetAudioClip(Music music)
    {
        foreach(MusicAudioClip musicAudioClip in MusicController.musicAudioClipArray)
        {
            if (musicAudioClip.music == music)
                return musicAudioClip.audioClip;
        }
        Debug.LogError("Music " + music + " not found!");
        return null;
    }
}
