using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] protected Animator _anim;

    protected bool _isPlayed = false;

    public virtual void Play()
    {
        if (_isPlayed)
            return;

        gameObject.SetActive(true);
    }
    public virtual void Stop()
    {
        gameObject.SetActive(false);
        _isPlayed = true;
    }
}
