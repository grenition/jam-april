using UnityEngine;

public struct AttackData
{
    public float Damage;
    public DamageType Type;
    public float MoveSpeed;
    public AttackColliderType ColliderType;
    public float KnockBackStrength;
    public AudioClip SlashClip;

    public AttackData(float damage, DamageType type,
        float moveSpeed, AttackColliderType colliderType, float knockbackStrength, 
        AudioClip clip = null)
    {
        Damage = damage;
        Type = type;
        MoveSpeed = moveSpeed;
        ColliderType = colliderType;
        KnockBackStrength = knockbackStrength;
        SlashClip = clip;
    }
}

public enum AttackColliderType
{
    Slash, Circle
} 
