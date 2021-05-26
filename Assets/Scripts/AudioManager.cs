using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager 
{

    public enum Music
    {
        Menu,
        Town,
        Water,
        Fire,
        Earth,
        Air
    }
    
    public static void PlayMusic(Music music, AudioSource audioSource)
    {
        audioSource.Stop();
        audioSource.clip = GetAudioClip(music);
        audioSource.Play();
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
