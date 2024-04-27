using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCapLifetime : GameLifetime
{
    public RedCap RedCap => _redCap;

    [SerializeField] private RedCap _redCap;

    protected override void Awake()
    {
        base.Awake();
        ServiceLocator.Register<RedCapLifetime>(this);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        ServiceLocator.Unregister<RedCapLifetime>();
    }

    public override void Play()
    {
        base.Play();
    }   
    public override void Stop()
    {
        base.Stop();
    }
}
