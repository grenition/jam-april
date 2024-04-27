using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SpeechSource : MonoBehaviour
{
    public UnityEvent OnSpeechStart;
    public UnityEvent OnSpeechEnd;

    [SerializeField] private Speech _speech;
    [SerializeField] private bool _playOneTime = true;

    private AudioController _controller;
    private UISubtitles _subtitles;
    private bool _isPlayed = false;

    private void Start()
    {
        _controller = ServiceLocator.Get<AudioController>();
        _subtitles = ServiceLocator.Get<UISubtitles>();
    }

    public void Play()
    {
        if (_speech == null)
            return;

        if (_playOneTime && _isPlayed)
            return;

        foreach (var speech in _speech.speeches)
            StartCoroutine(PlaySpeechClip(speech));

        StartCoroutine(CallSpeechEnd(_speech.duration));

        _isPlayed = true;

        OnSpeechStart?.Invoke();
    }
    public void Stop()
    {
        StopAllCoroutines();
    }
    private IEnumerator PlaySpeechClip(Speech.SpeechData speechData)
    {
        yield return new WaitForSeconds(speechData.startTime);
        _controller?.PlayOneShot(speechData.speechClip.clip, AudioSourceChannel.voices);
        _subtitles?.DrawSubtitles(speechData.speechClip.subtitles);
    }
    private IEnumerator CallSpeechEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnSpeechEnd?.Invoke();
    }
}
