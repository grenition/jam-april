public struct AttackData
{
    public float Damage;
    public DamageType Type;
    public float MoveSpeed;
    public AttackColliderType ColliderType;
    public float KnockBackStrength;

    public AttackData(float damage, DamageType type,
        float moveSpeed, AttackColliderType colliderType, float knockbackStrength)
    {
        Damage = damage;
        Type = type;
        MoveSpeed = moveSpeed;
        ColliderType = colliderType;
        KnockBackStrength = knockbackStrength;
    }
}

public enum AttackColliderType
{
    Slash, Circle
} 
