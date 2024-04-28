using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Unity.VisualScripting.Member;

public enum AudioSourceChannel
{
    voices,
    music,
    sfx
}

public class AudioController : MonoBehaviour
{
    public AudioSource Source => _voiceSource;

    [SerializeField] private AudioSource _voiceSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _musicSource2;

    [SerializeField] private float _musicFadeTime = 2f;

    private AudioSource _activeMusicSource;

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
                _voiceSource.Stop();
                _voiceSource.PlayOneShot(clip);
                break;
            case AudioSourceChannel.music:
                _voiceSource.PlayOneShot(clip);
                break;
            case AudioSourceChannel.sfx:
                _sfxSource.PlayOneShot(clip);
                break;
        }
    }
    public void PlayMusic(AudioClip music)
    {
        var source = _musicSource;
        var additionalSource = _musicSource2;

        if (_activeMusicSource == _musicSource)
        {
            source = _musicSource2;
            additionalSource = _musicSource;
        }

        _activeMusicSource = source;

        source.DOKill();
        additionalSource.DOKill();

        source.clip = music;
        source.Play();
        source.DOFade(1f, _musicFadeTime);

        additionalSource.DOFade(0f, _musicFadeTime)
            .OnComplete(()=> additionalSource.Stop());
    }
    public void StopMusic()
    {
        var source = _musicSource;
        var additionalSource = _musicSource2;

        source.DOKill();
        additionalSource.DOKill();

        source.DOFade(0f, _musicFadeTime)
            .OnComplete(() => source.Stop());
        additionalSource.DOFade(0f, _musicFadeTime)
            .OnComplete(() => additionalSource.Stop());
    }
}
