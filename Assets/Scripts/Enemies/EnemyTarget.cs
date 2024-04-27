using UnityEngine;

public struct EnemyTarget
{
    public Transform Transform;
    public IDamageable Damageable;

    public EnemyTarget(Transform transform, IDamageable damageable)
    {
        Transform = transform;
        Damageable = damageable;
    }
}
