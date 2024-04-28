using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAI : BaseEnemy
{
    [SerializeField] private float _runSpeed;
    [SerializeField] private GoToPointPattern _goToPointPattern;
    [SerializeField] private FollowTargetPattern _followTargetPattern;
    [SerializeField] private FindNearestTargetPattern _findNearestTargetPattern;

    [SerializeField] private float _maxDistance, _minDistance;

    public override void Hurt(GameObject source, AttackData data)
    {
        if(_findNearestTargetPattern.Working)
        {
            foreach(var target in _findNearestTargetPattern.PossibleTargets)
            {
                if(target == source)
                {
                    TrySetTarget(target);
                    break;
                }
            }
            _findNearestTargetPattern.StopPattern();
        }

        base.Hurt(source, data);
    }

    protected override void Update()
    {
        base.Update();


    }
}
