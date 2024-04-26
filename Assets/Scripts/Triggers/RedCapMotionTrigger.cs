using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedCapMotionTrigger : PlayerTrigger
{
    [SerializeField] private MovementSpline _movementSpline;

    private bool _isUsedBefore = false;
    protected override void OnPlayerTriggerEnter(Player player)
    {
        if (_isUsedBefore)
            return;

        _movementSpline.MoveAgent(RedCap.Instance.Agent);
    }
}
