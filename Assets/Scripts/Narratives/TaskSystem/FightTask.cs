using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTask : Task
{
    [SerializeField] private RedCapAnimationState _redCapState = RedCapAnimationState.none;
    [SerializeField] private float _animationDelay = 2f;
    [SerializeField] private float _fightTime = 20f;
    [SerializeField] private float _maxFightTime = 120f;
    [SerializeField] private float _healDelayAfterTask = 10f;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private AudioClip _fightMusic;
    public override void StartTask()
    {
        if (IsTaskActive)
            return;

        StartCoroutine(TaskCoroutine());
        StartCoroutine(FightTimeCoroutine());
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
    private IEnumerator FightTimeCoroutine()
    {
        yield return new WaitForSeconds(_maxFightTime);
        if (IsTaskActive)
        {
            CompleteTask();
            StopAllCoroutines();
        }
    }
    private IEnumerator TaskCoroutine()
    {
        if (IsTaskActive)
            yield break;

        var redCaplLfetime = ServiceLocator.Get<RedCapLifetime>();
        var redCapAnim = redCaplLfetime.RedCap.GetComponent<RedCapAnimations>();
        var audioController = ServiceLocator.Get<AudioController>();

        _enemySpawner.StartSpawner();

        audioController?.PlayMusic(_fightMusic);

        yield return new WaitForSeconds(_animationDelay);

        redCapAnim?.SetAnimState(_redCapState);

        yield return new WaitForSeconds(_fightTime - _animationDelay);

        _enemySpawner?.StopSpawner();
        yield return new WaitUntil(() => _enemySpawner.Enemies.Count == 0);

        redCapAnim?.SetAnimState(RedCapAnimationState.none);

        audioController?.StopMusic();

        CompleteTask();

        yield return new WaitForSeconds(_healDelayAfterTask);

        redCaplLfetime.Player.GetComponent<PlayerStats>().HealAll();
    }
}
