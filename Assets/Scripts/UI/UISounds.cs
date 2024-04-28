using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip _enterClip;
    [SerializeField] private AudioClip _clickClip;

    private AudioController _controller;


    private void Start()
    {
        _controller = ServiceLocator.Get<AudioController>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _controller?.PlayOneShot(_clickClip, AudioSourceChannel.sfx);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _controller?.PlayOneShot(_enterClip, AudioSourceChannel.sfx);
    }


}
