using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundAudioClip
{
    public AudioManager.Sound sound;
    public AudioClip audioClip;
}
public class SoundController : MonoBehaviour
{
    [SerializeField]
    SoundAudioClip[] _soundAudioClipArray;

    public static SoundAudioClip[] soundAudioClipArray;

    private void Awake()
    {
        soundAudioClipArray = _soundAudioClipArray;
        AudioManager.Initialize();
    }
}
