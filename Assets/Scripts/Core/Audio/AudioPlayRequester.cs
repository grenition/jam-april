using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayRequester : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    public void RequestAudioPlay()
    {
        var controller = ServiceLocator.Get<AudioController>();

        if (controller == null)
            return;

        controller.PlayMusic(_clip);
    }
}
