using UnityEngine;
using UnityEngine.AI;

public class BaseMeleeEnemy : BaseEnemy
{
    [SerializeField] private NavMeshQueryFilter _navMeshfilter;
    [SerializeField] private float _movementSpeed;

    private NavMeshPath _path;
    private int _pathIndex = 0;
    private Vector3 _nextPointToMove = Vector3.zero;
    private float _prevDistanceToPoint = -1f;

    private float _reCalculatePathTimer = 0;

    public EnemyTarget Target { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        var player = FindObjectOfType<PlayerStats>();
        Target = new EnemyTarget(player.transform, player);
    }

    protected virtual void Start()
    {
        _path = new();
        ReCalculatePath();
    }

    public bool TrySetTarget(GameObject target)
    {
        if(target.TryGetComponent<IDamageable>(out var damageable))
        {
            Target = new EnemyTarget(target.transform, damageable);
            return true;
        }
        return false;
    }

    public void ReCalculatePath()
    {
        NavMesh.CalculatePath(transform.position, Target.Transform.position,
            0b11111111, _path);

        if(_path == null || _path.corners.Length == 0)
        {
            _pathIndex = -1;
            return;
        }

        _pathIndex = 0;
        _nextPointToMove = _path.corners[0];
    }

    public void GoToNextPoint()
    {
        _prevDistanceToPoint = -1;
        _pathIndex++;
        if(_pathIndex >= _path.corners.Length)
        {
            _pathIndex = -1;
            return;
        }

        _nextPointToMove = _path.corners[_pathIndex];
    }

    protected override void Update()
    {
        base.Update();

        if(_reCalculatePathTimer > 0)
        {
            _reCalculatePathTimer -= Time.deltaTime;
        }
        else
        {
            _reCalculatePathTimer = 1;
        }
        
        if(_pathIndex >= 0)
        {
            var direction = _nextPointToMove - transform.position;
            direction = direction.normalized * _movementSpeed;
            Controller.Move(direction * Time.deltaTime);

            float dist = Vector3.Distance(transform.position, _nextPointToMove);
            if(_prevDistanceToPoint < 0 || dist < _prevDistanceToPoint)
            {
                _prevDistanceToPoint = dist;
            }
            else
            {
                GoToNextPoint();
            }
        }
    }
}
