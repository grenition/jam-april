using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _knockbackResistance;
    [SerializeField] private GravityObject _gravityObject;
    [SerializeField] private DamageIndicatorsPool _damageIndicatorsPool;

    public const float KNOCKBACK_SLOWDOWN = 4;

    private Vector3 _knockbackDirection = Vector3.zero;
    private float _knockbackStrength = 0;
    private CharacterController _controller;

    public float Health { get; protected set; }

    public float MaxHealth => _maxHealth;
    public GravityObject GravityObject => _gravityObject;

    protected virtual void Awake()
    {
        Health = MaxHealth;
        _controller = GetComponent<CharacterController>();
    }

    public void Knockback(Vector3 direction)
    {
        if (_knockbackResistance >= 100)
        {
            _knockbackDirection = Vector3.zero;
        }
        else
        {
            _knockbackDirection = direction.normalized;
            if(_knockbackResistance > 0)
            {
                _knockbackStrength = 5 / _knockbackResistance;
            }
            else
            {
                _knockbackStrength = 5;
            }
        }
    }

    public virtual void Hurt(GameObject source, DamageType type, float damage)
    {
        Health -= damage;

        var knockbackDirection = transform.position - source.transform.position;
        knockbackDirection.y = 0;
        Knockback(knockbackDirection);
        
        _damageIndicatorsPool.SpawnIndicator(damage, type);
        if(Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        if(_gravityObject.VelocityY != 0)
        {
            _controller.Move(Vector3.up * _gravityObject.VelocityY * Time.deltaTime);
        }
        if(_knockbackStrength > 0)
        {
            _controller.Move(_knockbackDirection * _knockbackStrength * Time.deltaTime);
            if(_gravityObject.OnLand)
                _knockbackStrength -= KNOCKBACK_SLOWDOWN * Time.deltaTime;
        }
    }
}
