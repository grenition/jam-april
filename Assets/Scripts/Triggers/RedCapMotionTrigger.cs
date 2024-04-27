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

        var redCapLifetime = ServiceLocator.Get<RedCapLifetime>();
        if (redCapLifetime == null)
            return;

        _movementSpline.MoveAgent(redCapLifetime.RedCap.Agent);

        _isUsedBefore = true;
    }
}
