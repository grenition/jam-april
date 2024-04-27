using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GoToPointPattern : EnemyPattern
{
    public Vector3 Point { get; private set; }
    public event Action PathCompleted;

    private int _pathIndex = -1;
    private Vector3 _nextPointToMove;

    private float _prevDistanceToPoint = -1;

    public override bool ReCalculatePath()
    {
        if (Path == null)
            return false;

        NavMesh.CalculatePath(transform.position, Point,
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

    public bool SetPoint(Vector3 pos)
    {
        var oldPoint = Point;
        Point = pos;
        if(ReCalculatePath())
        {
            return true;
        }
        else
        {
            Point = oldPoint;
            return false;
        }
    }

    public override bool ToNextPoint()
    {
        _prevDistanceToPoint = -1;
        _pathIndex++;
        if (_pathIndex >= Path.corners.Length)
        {
            _pathIndex = -1;
            PathCompleted?.Invoke();
            return false;
        }

        _nextPointToMove = Path.corners[_pathIndex];
        return true;
    }

    protected override void WorkUpdate()
    {
        if (_pathIndex == -1 || Enemy.IsStuck)
            return;

        var direction = _nextPointToMove - transform.position;
        direction = direction.normalized * Enemy.Speed;
        direction.y = 0;
        Enemy.Controller.Move(direction * Time.deltaTime);
        transform.LookAt(transform.position + direction * 10);

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
}
