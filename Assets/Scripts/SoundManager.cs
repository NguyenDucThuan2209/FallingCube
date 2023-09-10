using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Audio
{
    [SerializeField] string m_name;
    [SerializeField] AudioClip m_clip;
    public string Name => m_name;
    public AudioClip Clip => m_clip;
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_instance;
    public static SoundManager Instance => m_instance;

    [SerializeField] AudioSource m_musicSource;
    [SerializeField] AudioSource m_soundSource;
    [SerializeField] Audio[] m_musicAudio;
    [SerializeField] Audio[] m_soundAudio;

    public bool MusicState => m_musicSource.mute;
    public bool SoundState => m_soundSource.mute;

    private void Awake()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_instance = this;
    }
    private void Start()
    {
        PlayMusic("BackgroundMusic");
    }

    public void PlayMusic(string name)
    {
        var audio = Array.Find(m_musicAudio, audio => audio.Name == name);
        m_musicSource.clip = audio?.Clip;
        m_musicSource.Play();
    }
    public void PlaySound(string name)
    {
        var audio = Array.Find(m_soundAudio, audio => audio.Name == name);
        m_soundSource.PlayOneShot(audio?.Clip);
    }
    public void SetMusicVolume(float value)
    {
        m_musicSource.volume = value;
    }
    public void SetSoundVolume(float value)
    {
        m_soundSource.volume = value;
    }
    public void SetMusicState(bool isMute)
    {
        m_musicSource.mute = isMute;
    }
    public void SetSoundState(bool isMute)
    {
        m_soundSource.mute = isMute;
    }
}
