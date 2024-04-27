using System.Collections.Generic;
using UnityEngine;

public class FindNearestTargetPattern : EnemyPattern
{
    public readonly List<GameObject> PossibleTargets = new();

    private float _checkDistanceTimer = 0;
    private float _checkDistanceCooldown = 1f;

    public override bool ReCalculatePath()
    {
        return false;
    }

    public override bool ToNextPoint()
    {
        return false;
    }

    public void FindTarget()
    {
        if (PossibleTargets.Count == 0)
            return;

        float minDist = Vector3.Distance(transform.position, PossibleTargets[0].transform.position);
        GameObject target = PossibleTargets[0];
        for(int i = 1; i < PossibleTargets.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, PossibleTargets[i].transform.position);
            if(dist < minDist)
            {
                minDist = dist;
                target = PossibleTargets[i];
            }
        }

        Enemy.TrySetTarget(target);
    }

    protected override void WorkUpdate()
    {
        if(_checkDistanceTimer > 0)
        {
            _checkDistanceTimer -= Time.deltaTime;
        }
        else
        {
            _checkDistanceTimer = _checkDistanceCooldown;
            FindTarget();
        }
    }
}
