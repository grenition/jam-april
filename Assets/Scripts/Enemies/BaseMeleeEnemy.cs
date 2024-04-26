using UnityEngine;

public class BaseMeleeEnemy : BaseEnemy
{
    public EnemyTarget Target { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        var player = FindObjectOfType<PlayerStats>();
        Target = new EnemyTarget(player.transform, player);
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

    protected override void Update()
    {
        base.Update();

    }
}
