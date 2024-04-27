using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth, _speed;
    [SerializeField] private float _knockbackResistance;
    [SerializeField] private GravityObject _gravityObject;
    [SerializeField] private DamageIndicatorsPool _damageIndicatorsPool;
    [SerializeField] private float _hurtStamina, _stuckTime;

    public const float KNOCKBACK_SLOWDOWN = 4;

    private Vector3 _knockbackDirection = Vector3.zero;
    private float _knockbackStrength = 0;
    private Coroutine _stuckCor;

    public CharacterController Controller { get; private set; }

    public float Health { get; protected set; }

    public float MaxHealth => _maxHealth;
    public float Speed => _speed;
    public GravityObject GravityObject => _gravityObject;
    public EnemyTarget Target { get; private set; }
    public bool IsStuck { get; private set; }

    protected virtual void Awake()
    {
        Health = MaxHealth;
        Controller = GetComponent<CharacterController>();
        var player = FindObjectOfType<PlayerStats>();
        Target = new EnemyTarget(player.transform, player);
        _damageIndicatorsPool.SetParentAndOffset(transform, Vector3.up * 1.4f);
        _damageIndicatorsPool.Initialize(_maxHealth);
    }

    public bool TrySetTarget(GameObject target)
    {
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            Target = new EnemyTarget(target.transform, damageable);
            return true;
        }
        return false;
    }

    public void Knockback(Vector3 direction, float knockBackStrength)
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
                _knockbackStrength = knockBackStrength / _knockbackResistance;
            }
            else
            {
                _knockbackStrength = knockBackStrength;
            }
        }
    }

    private void Stuck()
    {
        if(_stuckCor != null)
        {
            StopCoroutine(_stuckCor);
        }
        _stuckCor = StartCoroutine(StuckIE());
    }

    private IEnumerator StuckIE()
    {
        IsStuck = true;
        yield return new WaitForSeconds(_stuckTime);
        IsStuck = false;
    }

    public virtual void Hurt(GameObject source, AttackData data)
    {
        Health -= data.Damage;

        var knockbackDirection = transform.position - source.transform.position;
        knockbackDirection.y = 0;
        Knockback(knockbackDirection, data.KnockBackStrength);
        
        _damageIndicatorsPool.SpawnIndicator(data.Damage, data.Type, Health);

        if(Health <= 0)
        {
            Die();
        }
        else if(_damageIndicatorsPool.TotalDamage >= _hurtStamina)
        {
            Stuck();
        }
    }

    public virtual void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        if (_gravityObject.VelocityY != 0)
        {
            Controller.Move(Vector3.up * _gravityObject.VelocityY * Time.deltaTime);
        }
        if(_knockbackStrength > 0)
        {
            Controller.Move(_knockbackDirection * _knockbackStrength * Time.deltaTime);
            if(_gravityObject.OnLand)
                _knockbackStrength -= KNOCKBACK_SLOWDOWN * Time.deltaTime;
        }
    }
}
