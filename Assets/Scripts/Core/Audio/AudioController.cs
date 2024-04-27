using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public AudioSource Source => _source;

    [SerializeField] private AudioSource _source;


    private void Awake()
    {
        ServiceLocator.Register<AudioController>(this);
    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<AudioController>();
    }

    public void PlayOneShot(AudioClip clip)
    {
        if(clip == null)
        {
            Debug.LogError("Clip is null");
            return;
        }
        _source.PlayOneShot(clip);
    }
}
