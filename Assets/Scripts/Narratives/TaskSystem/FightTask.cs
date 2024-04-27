using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTask : Task
{
    [SerializeField] private RedCapAnimationState _redCapState = RedCapAnimationState.none;
    [SerializeField] private float _animationDelay = 2f;
    [SerializeField] private float _fightTime = 20f;
    [SerializeField] private EnemySpawner _enemySpawner;
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

        var redCaplLfetime = ServiceLocator.Get<RedCapLifetime>();
        var redCapAnim = redCaplLfetime.RedCap.GetComponent<RedCapAnimations>();

        _enemySpawner.StartSpawner();

        yield return new WaitForSeconds(_animationDelay);

        redCapAnim?.SetAnimState(_redCapState);

        yield return new WaitForSeconds(_fightTime - _animationDelay);

        _enemySpawner?.StopSpawner();
        yield return new WaitUntil(() => _enemySpawner.Enemies.Count == 0);

        redCapAnim?.SetAnimState(RedCapAnimationState.none);

        CompleteTask();
    }
}
