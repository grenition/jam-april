using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerFighting : MonoBehaviour
{
    [SerializeField] private Transform _attackCenter;
    [SerializeField] private Vector3 _attackColliderSize;
    [SerializeField] private float _attackSphereRadius;
    [SerializeField] private LayerMask _weaponColliderMask;
    [SerializeField] private float _mobilityInAttack;

    [Header("Sounds")]
    [SerializeField] private AudioClip _quickAttackClip;
    [SerializeField] private AudioClip _quickAttack2Clip, _heavyAttackClip;

    public const KeyCode QUICK_ATTACK_KEY = KeyCode.Mouse0;
    public const KeyCode SLOW_ATTACK_KEY = KeyCode.Mouse1;
    public const KeyCode BLOCK_KEY = KeyCode.Q;

    private PlayerMovement _movement;
    private KeyCode _lastAttackKeyPressed = KeyCode.None;

    private Vector3 _slashMoveVector;
    private bool _isSlashMoving = false;
    private float _slashMoveStrength = 0;

    public readonly List<AttackData> AllAttacks = new();

    public bool IsBlocking { get; private set; }

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();

        #region AttacksData
        AllAttacks.Add(new(7, DamageType.QUICK_ATTACK, 4f, AttackColliderType.Slash, 5,
            _quickAttackClip));
        AllAttacks.Add(new(12, DamageType.HEAVY_ATTACK, 5f, AttackColliderType.Circle, 8,
            _heavyAttackClip));
        AllAttacks.Add(new(7, DamageType.QUICK_ATTACK, 4f, AttackColliderType.Circle, 7,
            _quickAttack2Clip));
        #endregion
    }

    private void QuickAttack()
    {
        _movement.OnAnimation = true;

        _movement.Animator.SetHeavyAttack(false);
        _movement.Animator.SetState(PlayerAnimatorState.Attack);
    }

    private void SlowAttack()
    {
        _movement.OnAnimation = true;

        _movement.Animator.SetHeavyAttack(true);
        _movement.Animator.SetState(PlayerAnimatorState.Attack);
    }

    public void StartBuffer()
    {
        _lastAttackKeyPressed = KeyCode.None;
    }

    public void MoveBeforeAttack(int attackIndex)
    {
        if (_movement.LastFrameInputVector.magnitude > 0)
        {
            _isSlashMoving = true;
            _slashMoveVector = _movement.LastFrameInputVector.normalized;
            _slashMoveStrength = AllAttacks[attackIndex].MoveSpeed;
        }
    }

    private Collider[] GetColliders(AttackColliderType type)
    {
        if (type == AttackColliderType.Slash)
        {
            return Physics.OverlapBox(
            _attackCenter.position, _attackColliderSize / 2,
            transform.rotation, _weaponColliderMask);
        }
        else
        {
            return Physics.OverlapSphere(
                transform.position, _attackSphereRadius,
                _weaponColliderMask);
        }
    }

    public void Attack(int attackIndex)
    {
        _isSlashMoving = false;
        float damage = AllAttacks[attackIndex].Damage;
        DamageType type = AllAttacks[attackIndex].Type;
        _movement.Animator.SetState(PlayerAnimatorState.Idle);

        var colliders = GetColliders(AllAttacks[attackIndex].ColliderType);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out var idamageable))
            {
                idamageable.Hurt(gameObject, AllAttacks[attackIndex]);
            }
        }

        CheckBuffer();
    }

    public void CheckBuffer()
    {
        if (_lastAttackKeyPressed != KeyCode.None)
        {
            _movement.transform.LookAt(_movement.transform.position + _movement.LastFrameInputVector * 10);
            if (_lastAttackKeyPressed == QUICK_ATTACK_KEY)
            {
                QuickAttack();
            }
            else if (_lastAttackKeyPressed == SLOW_ATTACK_KEY)
            {
                SlowAttack();
            }
            else if (_lastAttackKeyPressed == PlayerMovement.DASH_KEY)
            {
                _movement.Dash(_movement.LastFrameInputVector);
            }
        }
    }

    public void EndAttack()
    {
        _movement.OnAnimation = false;

    }

    public void Stun()
    {
        _isSlashMoving = false;
        _movement.Animator.SetState(PlayerAnimatorState.Stunning);
        _movement.OnAnimation = false;
    }

    private void Update()
    {
        if (_movement.Stats.IsStuck || !_movement.GravityObject.OnLand)
            return;

        if (_isSlashMoving)
        {
            _slashMoveVector = Vector3.Lerp(_slashMoveVector,
                _movement.LastFrameInputVector.normalized, Time.deltaTime * _mobilityInAttack);

            if (_movement.LastFrameInputVector.magnitude == 0)
                _isSlashMoving = false;
            _movement.Controller.Move(_slashMoveVector * _slashMoveStrength * Time.deltaTime);
        }

        if (!IsBlocking)
        {
            if (Input.GetKeyDown(QUICK_ATTACK_KEY) && !_movement.OnAnimation)
            {
                QuickAttack();
            }
            else if (Input.GetKeyDown(SLOW_ATTACK_KEY) && !_movement.OnAnimation)
            {
                SlowAttack();
            }
            else
            {
                var allBufferedKeys = new KeyCode[] { SLOW_ATTACK_KEY, QUICK_ATTACK_KEY,
                PlayerMovement.DASH_KEY };

                foreach (var key in allBufferedKeys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        _lastAttackKeyPressed = key;
                        break;
                    }
                }
            }
        }

        IsBlocking = Input.GetKey(BLOCK_KEY) && !_movement.OnAnimation;
        if (IsBlocking)
        {
            _movement.Animator.SetState(PlayerAnimatorState.Blocking);
        }
    }
}

