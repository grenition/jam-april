using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMotionPlayerTrigger : PlayerTrigger
{
    [SerializeField] private MovementSpline _movementSpline;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private bool _oneTimeUse = true;

    private bool _isUsedBefore = false;

    protected override void OnPlayerTriggerEnter(Player player)
    {
        if (_oneTimeUse && _isUsedBefore)
            return; 
        _movementSpline.MoveAgent(_agent);
    }
}
