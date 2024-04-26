using UnityEngine;

public interface IDamageable
{
    void Hurt(GameObject source, DamageType type, float damage);
}
