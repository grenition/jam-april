using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioSourceChannel
{
    voices,
    music
}

public class AudioController : MonoBehaviour
{
    public AudioSource Source => _voiceSource;

    [SerializeField] private AudioSource _voiceSource;
    [SerializeField] private AudioSource _musicSorce;

    private void Awake()
    {
        ServiceLocator.Register<AudioController>(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<AudioController>();
    }

    public void PlayOneShot(AudioClip clip, AudioSourceChannel sourceType)
    {
        if(clip == null)
        {
            Debug.LogError("Clip is null");
            return;
        }

        switch (sourceType)
        {
            case AudioSourceChannel.voices:
                _voiceSource.PlayOneShot(clip);
                break;
            case AudioSourceChannel.music:
                _voiceSource.PlayOneShot(clip);
                break;
        }
    }
}
