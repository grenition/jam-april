using System.Collections;
using UnityEngine;

[RequireComponent (typeof(PlayerMovement))]
public class PlayerFighting : MonoBehaviour
{
    [SerializeField] private Transform _attackCenter;
    [SerializeField] private Vector3 _attackColliderSize;
    [SerializeField] private float _preQuickAttackTime, _quickAttackTime;
    [SerializeField] private float _preSlowAttackTime, _slowAttackTime;
    [SerializeField] private float _quickAttackDamage, _slowAttackDamage;
    [SerializeField] private float _attackMoveLength;
    [SerializeField] private LayerMask _weaponColliderMask;

    [SerializeField] private GameObject TEST_slashEffect;

    public const KeyCode QUICK_ATTACK_KEY = KeyCode.Mouse0;
    public const KeyCode SLOW_ATTACK_KEY = KeyCode.Mouse1;
    public const KeyCode BLOCK_KEY = KeyCode.Q;

    private PlayerMovement _movement;
    private Coroutine _attackCor;
    private KeyCode _lastAttackKeyPressed = KeyCode.None;

    public bool IsBlocking { get; private set; }

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void QuickAttack()
    {
        _movement.OnAnimation = true;
        if(_attackCor != null)
        {
            StopCoroutine(_attackCor);
        }
        _attackCor = StartCoroutine(AttackIE(
            _preQuickAttackTime, _quickAttackTime, _quickAttackDamage, DamageType.QUICK_ATTACK
            ));
    }

    private void SlowAttack()
    {
        _movement.OnAnimation = true;
        if (_attackCor != null)
        {
            StopCoroutine(_attackCor);
        }
        _attackCor = StartCoroutine(AttackIE(
            _preSlowAttackTime, _slowAttackTime, _slowAttackDamage, DamageType.HEAVY_ATTACK
            ));
    }

    private IEnumerator AttackIE(float preTime, float attackTime, float damage, DamageType type)
    {
        yield return new WaitForSeconds(preTime);
        _lastAttackKeyPressed = KeyCode.None;
        TEST_slashEffect.SetActive(true);
        
        if(_movement.LastFrameInputVector.magnitude > 0)
        {
            _movement.Controller.Move(_movement.LastFrameInputVector * _attackMoveLength);
        }

        var colliders = Physics.OverlapBox(
            _attackCenter.position, _attackColliderSize / 2,
            transform.rotation, _weaponColliderMask);
        foreach(var collider in colliders)
        {
            if(collider.TryGetComponent<IDamageable>(out var idamageable))
            {
                idamageable.Hurt(gameObject, type, damage);
            }
        }
        yield return new WaitForSeconds(attackTime);
        TEST_slashEffect.SetActive(false);
        _movement.OnAnimation = false;

        if(_lastAttackKeyPressed != KeyCode.None)
        {
            _movement.transform.LookAt(_movement.transform.position + _movement.LastFrameInputVector * 10);
            if(_lastAttackKeyPressed == QUICK_ATTACK_KEY)
            {
                QuickAttack();
            }
            else if(_lastAttackKeyPressed == SLOW_ATTACK_KEY)
            {
                SlowAttack();
            }
            else if(_lastAttackKeyPressed == PlayerMovement.DASH_KEY)
            {
                _movement.Dash(_movement.LastFrameInputVector);
            }
        }
    }

    private void Update()
    {
        if (_movement.Stats.IsStuck)
            return;

        if(Input.GetKeyDown(QUICK_ATTACK_KEY) && !_movement.OnAnimation)
        {
            QuickAttack();
        }
        else if(Input.GetKeyDown(SLOW_ATTACK_KEY) && !_movement.OnAnimation)
        {
            SlowAttack();
        }
        else
        {
            var allBufferedKeys = new KeyCode[] { SLOW_ATTACK_KEY, QUICK_ATTACK_KEY,
                PlayerMovement.DASH_KEY };

            foreach(var key in allBufferedKeys)
            {
                if(Input.GetKeyDown(key))
                {
                    _lastAttackKeyPressed = key;
                    break;
                }
            }
        }

        IsBlocking = Input.GetKey(BLOCK_KEY) && !_movement.OnAnimation;
    }
}
