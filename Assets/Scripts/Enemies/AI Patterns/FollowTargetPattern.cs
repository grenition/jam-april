using UnityEngine;
using UnityEngine.AI;

public class FollowTargetPattern : EnemyPattern
{
    private int _pathIndex = -1;
    private Vector3 _nextPointToMove;
    private float _reCalculatePathTimer = 0;

    private float _prevDistanceToPoint = -1;

    public const float RECALCULATE_PATH_COOLDOWN = 1f;
    public const float TARGET_CLOSE_DISTANCE = 4;
    public const float TARGET_TOO_CLOSE_DISTANCE = 2;

    public bool IsPlayerNear { get; private set; }

    public override bool ReCalculatePath()
    {
        NavMesh.CalculatePath(transform.position, Enemy.Target.Transform.position,
            NavMesh.AllAreas, Path);

        if (Path == null || Path.corners.Length == 0)
        {
            _pathIndex = -1;
            return false;
        }

        _pathIndex = 0;
        _nextPointToMove = Path.corners[0];

        return true;
    }

    public override bool ToNextPoint()
    {
        _prevDistanceToPoint = -1;
        _pathIndex++;
        if(_pathIndex >= Path.corners.Length)
        {
            _pathIndex = -1;
            return false;
        }

        _nextPointToMove = Path.corners[_pathIndex];
        return true;
    }

    private void FollowByLine()
    {
        var direction = Enemy.Target.Transform.position - transform.position;
        direction = direction.normalized * Enemy.Speed;
        Enemy.Controller.Move(direction * Time.deltaTime);
    }

    private void FollowByNavMesh()
    {
        if (_pathIndex == -1)
            return;

        var direction = _nextPointToMove - transform.position;
        direction = direction.normalized * Enemy.Speed;
        Enemy.Controller.Move(direction * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, _nextPointToMove);
        if (_prevDistanceToPoint == -1 || dist < _prevDistanceToPoint)
        {
            _prevDistanceToPoint = dist;
        }
        else
        {
            ToNextPoint();
        }
    }

    protected override void WorkUpdate()
    {
        //Is player near
        float dist = Vector3.Distance(transform.position, Enemy.Target.Transform.position);
        if (IsPlayerNear && dist >= TARGET_CLOSE_DISTANCE)
        {
            ReCalculatePath();
        }
        IsPlayerNear = dist < TARGET_CLOSE_DISTANCE;

        //Update nav mesh path
        if (_reCalculatePathTimer > 0)
        {
            _reCalculatePathTimer -= Time.deltaTime;
        }
        else
        {
            _reCalculatePathTimer = RECALCULATE_PATH_COOLDOWN;
            if(!IsPlayerNear)
            {
                ReCalculatePath();
            }
        }

        //Move enemy
        if(dist < TARGET_TOO_CLOSE_DISTANCE)
        {
            return;
        }
        else if(IsPlayerNear)
        {
            FollowByLine();
        }
        else
        {
            FollowByNavMesh();
        }
    }
}
