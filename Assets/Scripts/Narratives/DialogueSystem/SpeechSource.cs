using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeechSource : MonoBehaviour
{
    [SerializeField] private Speech _speech;
    [SerializeField] private bool _playOneTime = true;

    private AudioController _controller;
    private bool _isPlayed = false;
    private void Start()
    {
        _controller = ServiceLocator.Get<AudioController>();
    }

    public void Play()
    {
        if (_speech == null)
            return;

        if (_playOneTime && _isPlayed)
            return;

        foreach (var speech in _speech.speeches)
            StartCoroutine(PlaySpeechClip(speech));

        _isPlayed = true;
    }
    public void Stop()
    {
        StopAllCoroutines();
    }
    private IEnumerator PlaySpeechClip(Speech.SpeechData speechData)
    {
        yield return new WaitForSeconds(speechData.startTime);
        _controller.PlayOneShot(speechData.speechClip.clip);
    }
}
