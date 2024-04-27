using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTask : Task
{
    [SerializeField] private bool _redCapGathering = true;
    [SerializeField] private float _fightTime = 20f;
    public override void StartTask()
    {
        if (IsTaskActive)
            return;

        StartCoroutine(TaskCoroutine());
        base.StartTask();
    }
    public override void ForceStopTask()
    {
        StopAllCoroutines();
        base.ForceStopTask();
    }
    public override void CompleteTask()
    {
        base.CompleteTask();
    }
    private IEnumerator TaskCoroutine()
    {
        if (IsTaskActive)
            yield break;

        var lifetime = ServiceLocator.Get<GameLifetime>();
        var player = lifetime != null ? lifetime.Player : null;

        if (player == null)
            yield break;

        yield return new WaitForSeconds(_fightTime);

        CompleteTask();
    }
}
