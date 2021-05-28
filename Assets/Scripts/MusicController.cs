using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MusicAudioClip
{
    public AudioManager.Music music;
    public AudioClip audioClip;
}
public class MusicController : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    MusicAudioClip[] _musicAudioClipArray;

    public static MusicAudioClip[] musicAudioClipArray;

    private void Awake()
    {
        musicAudioClipArray = _musicAudioClipArray;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "PuebloFulir")
            AudioManager.PlayMusic(AudioManager.Music.Town, audioSource);
        else
            AudioManager.PlayMusic(AudioManager.Music.Menu, audioSource);
    }

    private void OnEnable()
    {
        AreaTrigger.EnteredArea += HandleEnteredArea;
        Player.PlayerKilled += StopMusic;
        Player.PlayerRespawned += HandleRespawn;
    }

    private void OnDisable()
    {
        AreaTrigger.EnteredArea -= HandleEnteredArea;
        Player.PlayerKilled -= StopMusic;
        Player.PlayerRespawned -= HandleRespawn;
    }

    private void HandleEnteredArea(string area)
    {
        switch(area)
        {
            case "Water":
                AudioManager.PlayMusic(AudioManager.Music.Water, audioSource);
            break;
            case "Fire":
                AudioManager.PlayMusic(AudioManager.Music.Fire, audioSource);
            break;
            case "Earth":
                AudioManager.PlayMusic(AudioManager.Music.Earth, audioSource);
            break;
            case "Air":
                AudioManager.PlayMusic(AudioManager.Music.Air, audioSource);
            break;
            case "Town":
                AudioManager.PlayMusic(AudioManager.Music.Town, audioSource);
            break;
        }
    }
    private void HandleRespawn()
    {
        AudioManager.PlayMusic(AudioManager.Music.Town, audioSource);
    }
    private void StopMusic()
    {
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
