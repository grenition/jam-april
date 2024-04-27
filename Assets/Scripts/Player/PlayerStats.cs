using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private DamageIndicatorsPool _damageIndicators;
    [SerializeField] private float _hurtStamina, _stuckTime, _stuckImmuneTime;
    [SerializeField] private float _knockbackResistance;
    [SerializeField] private float _shieldStamina, _shieldStaminaSpeed;

    private float _curHealth, _curShieldStamina;
    private Coroutine _stuckCor;

    public bool IsStuck { get; private set; }
    public bool IsImmuneToStuck { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerFighting Fighting { get; private set; }

    private void Start()
    {
        _curHealth = _maxHealth;
        _damageIndicators.SetParentAndOffset(transform, Vector3.up);
        Movement = FindObjectOfType<PlayerMovement>();
        Fighting = FindObjectOfType<PlayerFighting>();
    }

    public void Stuck()
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
        IsImmuneToStuck = true;
        yield return new WaitForSeconds(_stuckTime);
        IsStuck = false;
        yield return new WaitForSeconds(_stuckImmuneTime);
        IsImmuneToStuck = false;
    }

    private bool IsLookingForObject(Transform go)
    {
        var toTarget = go.position - transform.position;
        float angle = Vector3.Angle(toTarget, transform.forward);
        return angle < 70;
    }

    public void Hurt(GameObject source, DamageType type, float damage)
    {
        if(Fighting.IsBlocking && IsLookingForObject(source.transform))
        {
            _curShieldStamina += damage;
            damage /= 10;
        }

        _damageIndicators.SpawnIndicator(damage, type);
        _curHealth -= damage;

        if(_curHealth <= 0)
        {
            Die();
        }
        else if (_damageIndicators.TotalDamage >= _hurtStamina && !IsImmuneToStuck)
        {
            Stuck();
            var direction = transform.position - source.transform.position;
            direction.y = 0;
            Movement.Knockback(_knockbackResistance > 0 ? 5 / _knockbackResistance : 5, direction);
        }
        else if(_curShieldStamina > _shieldStamina)
        {
            Stuck();
        }
    }

    private void Update()
    {
        if(_curShieldStamina > 0)
        {
            _curShieldStamina = Mathf.Clamp(_curShieldStamina -  _shieldStaminaSpeed * Time.deltaTime,
                0, _shieldStamina);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
