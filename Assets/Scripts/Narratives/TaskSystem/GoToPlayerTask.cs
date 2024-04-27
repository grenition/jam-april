using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayerTask : Task
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _threshholdDistance = 5f;
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

        yield return new WaitUntil(() => Vector3.Distance(player.transform.position, _endPoint.position) < _threshholdDistance);

        CompleteTask();
    }

    private void OnDrawGizmos()
    {
        if (_endPoint == null)
            return;
        Gizmos.color = new Color(0, 1f, 0f, 1f);
        Gizmos.DrawWireSphere(_endPoint.position, _threshholdDistance);
    }
}
