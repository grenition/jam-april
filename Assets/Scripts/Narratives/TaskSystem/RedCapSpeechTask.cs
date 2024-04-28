using System.Collections;
using UnityEngine;

public class RedCapSpeechTask : Task
{
    [SerializeField] private SpeechSource _speechSource;
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

        if (_speechSource == null || _speechSource.Speech == null)
            yield break;

        _speechSource.Play();

        var redCapLifetime = ServiceLocator.Get<RedCapLifetime>();
        var animations = redCapLifetime.RedCap.GetComponent<RedCapAnimations>();

        animations?.SetAnimState(RedCapAnimationState.talking);

        yield return new WaitForSeconds(_speechSource.Speech.duration);

        animations?.SetAnimState(RedCapAnimationState.none);

        CompleteTask();
    }
}
