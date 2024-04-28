using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private DamageIndicatorsPool _damageIndicators;
    [SerializeField] private float _hurtStamina, _stuckTime, _stuckImmuneTime;
    [SerializeField] private float _knockbackResistance;
    [SerializeField] private float _shieldStamina, _shieldStaminaSpeed;

    [SerializeField] private Slider _staminaBar, _staminaBar2;

    public event Action Died, Hurted;

    private float _curHealth, _curShieldStamina;
    private Coroutine _stuckCor;
    private GameLifetime _gameLifetime;

    public bool IsStuck { get; private set; }
    public bool IsImmuneToStuck { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerFighting Fighting { get; private set; }

    private void Start()
    {
        _maxHealth = Difficulty.GetPlayerHealth();
        _curHealth = _maxHealth;
        _damageIndicators.SetParentAndOffset(transform, Vector3.up);
        Movement = FindObjectOfType<PlayerMovement>();
        Fighting = FindObjectOfType<PlayerFighting>();
        _damageIndicators.Initialize(_maxHealth);
        _staminaBar.maxValue = _shieldStamina;
        _staminaBar.value = 0f;
        _staminaBar2.maxValue = _shieldStamina;
        _staminaBar2.value = 0f;

        _gameLifetime = ServiceLocator.Get<GameLifetime>();
    }

    public void Stuck()
    {
        if (_stuckCor != null)
        {
            StopCoroutine(_stuckCor);
        }
        Fighting.Stun();
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

    public void Hurt(GameObject source, AttackData data)
    {
        Hurted?.Invoke();
        if (Fighting.IsBlocking && IsLookingForObject(source.transform))
        {
            _curShieldStamina += data.Damage;
            if (_curShieldStamina > _shieldStamina || data.Type == DamageType.HEAVY_ATTACK)
            {
                Stuck();
            }
            else
            {
                data.Damage /= 10;
            }
        }

        _damageIndicators.SpawnIndicator(data.Damage, data.Type, _curHealth);
        _curHealth -= data.Damage;

        if (_curHealth <= 0)
        {
            Die();
        }
        else if (_damageIndicators.TotalDamage >= _hurtStamina && !IsImmuneToStuck)
        {
            Stuck();
            var direction = transform.position - source.transform.position;
            direction.y = 0;
            Movement.Knockback(_knockbackResistance > 0 ?
                data.KnockBackStrength / _knockbackResistance : data.KnockBackStrength,
                direction);
        }
    }

    private void Update()
    {
        if (_curShieldStamina > 0)
        {
            _curShieldStamina = Mathf.Clamp(_curShieldStamina - _shieldStaminaSpeed * Time.deltaTime,
                0, _shieldStamina);
            _staminaBar.value = _curShieldStamina;
            _staminaBar2.value = _curShieldStamina;
        }
    }

    public void Die()
    {
        Movement.Animator.SetState(PlayerAnimatorState.Die);
        Died?.Invoke();
        StartCoroutine(DieIE());
    }

    private IEnumerator DieIE()
    {
        yield return new WaitForSeconds(2);
        AlmostDie();
    }

    private void AlmostDie()
    {
        StopAllCoroutines();
        Destroy(gameObject);

        _gameLifetime?.Loose(LooseReason.die);
    }

    public void Heal(float value)
    {
        float diff = Mathf.Min(_maxHealth - _curHealth, value);
        _curHealth = Mathf.Clamp(_curHealth + value, 0, _maxHealth);
        _damageIndicators.SpawnIndicator(-diff, DamageType.HEAL, _curHealth);
    }

    public void HealAll()
    {
        Heal(10000);
    }
}

