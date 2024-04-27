using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAI : BaseEnemy
{
    [SerializeField] private float _attackCooldown, _beforeAttackNotificationTime;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] private float _damage;
    [SerializeField] private GoToPointPattern _goToPointPattern;
    [SerializeField] private float _maxTargetingDistance, _minTargetingDistance;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _arrowInHand, _arrowProjectile;

    private float _attackTimer = 0, _checkPlayerDistTimer = 0;
    private bool _isTargeting = false, _isPreFire = false, _onAnimation = false;
    private bool _isDie = false;
    private Vector3 _preFireDirection = Vector3.zero;
    private Vector3 _lastBadPose = Vector3.zero;
    private Vector3 _prevPos = Vector3.zero;
    private int _comboOfArrows = 0;

    private Coroutine _hurtCor;

    private const float CHECK_PLAYER_DIST_COOLDOWN = 1;
    private const int COUNT_OF_ARROWS_ON_ONE_POINT = 3;
    private const string STATE = "State";

    protected override void Awake()
    {
        base.Awake();
        _goToPointPattern.Initialize(this);
    }

    private void Start()
    {
        StopTargeting();
        _goToPointPattern.PathCompleted += () =>
        _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Idle);
        _arrowProjectile.transform.SetParent(null);
    }

    private void StartTargeting()
    {
        _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Targeting);
        _arrowInHand.SetActive(true);
        _isTargeting = true;
        _attackTimer = _attackCooldown;
        _isPreFire = false;
        HideLineRenderer();
        _goToPointPattern.StopPattern();
    }

    private void StopTargeting()
    {
        _arrowInHand.SetActive(false);
        _comboOfArrows = 0;
        _isTargeting = false;
        _isPreFire = false;
        HideLineRenderer();
        FindGoodPos();
    }

    private void FindGoodPos()
    {
        var initDirection = (transform.position - Target.Transform.position).normalized;
        for (int attempts = 0; attempts < 15; attempts++)
        {
            float range = 60;
            if (attempts > 4)
                range = 90;
            else if (attempts > 6)
                range = 130;
            else if(attempts > 9)
                range = 180;

            float newAngle = Random.Range(-range, range) * Mathf.Deg2Rad;
            float length = (_minTargetingDistance + _maxTargetingDistance) / 2;
            var direction = new Vector3(
                initDirection.x * Mathf.Cos(newAngle) - initDirection.z * Mathf.Sin(newAngle),
                0,
                initDirection.x * Mathf.Sin(newAngle) + initDirection.z * Mathf.Cos(newAngle));
            var pos = Target.Transform.position + direction * length;

            if(CanSeeTargetFrom(pos) && _goToPointPattern.SetPoint(pos))
            {
                _goToPointPattern.StartPattern();
                return;
            }
        }
    }

    public override void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieIE());
    }

    private IEnumerator DieIE()
    {
        _isDie = true;
        _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Die);
        yield return null;
        _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Die);
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    public override void Hurt(GameObject source, AttackData data)
    {
        if (_isDie)
            return;

        base.Hurt(source, data);
        _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Hurt);
        StopTargeting();
        if (_hurtCor != null)
            StopCoroutine(_hurtCor);
        _hurtCor = StartCoroutine(HurtIE());
    }

    private IEnumerator HurtIE()
    {
        _onAnimation = true;
        yield return new WaitForSeconds(.5f);
        _onAnimation = false;
        _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Idle);
    }

    private void HideLineRenderer()
    {
        Color color = new(0, 0, 0, 0);
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    private void Fire()
    {
        HideLineRenderer();
        _isPreFire = false;

        var ray = new Ray(transform.position, _preFireDirection);
        if(Physics.Raycast(ray, out var hit, _maxTargetingDistance, _attackMask))
        {
            if(hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Hurt(gameObject, new(
                    _damage, DamageType.QUICK_ATTACK, 0, AttackColliderType.Slash, 5));
            }
        }

        _arrowProjectile.SetActive(true);
        _arrowProjectile.transform.position = transform.position;
        _arrowProjectile.transform.LookAt(hit.point);
        _arrowProjectile.transform.DOMove(hit.point, (hit.point - transform.position).magnitude / 40);
        StartCoroutine(ArrowIE());
    }

    private IEnumerator ArrowIE()
    {
        yield return new WaitForSeconds(.5f);
        _arrowProjectile.SetActive(false);
    }

    private void PreFire()
    {
        if(!CanSeeTarget())
        {
            StopTargeting();
            return;
        }

        _isPreFire = true;
        _preFireDirection = (Target.Transform.position - transform.position).normalized;

        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + _preFireDirection * _maxTargetingDistance);
    }

    private bool CanSeeTarget()
    {
        var ray = new Ray(transform.position, Target.Transform.position - transform.position);
        if(Physics.Raycast(ray, out var hit, _maxTargetingDistance, _attackMask))
        {
            return hit.collider.TryGetComponent<PlayerStats>(out _);
        }
        return false;
    }

    private bool CanSeeTargetFrom(Vector3 fromPos)
    {
        var ray = new Ray(fromPos, Target.Transform.position - fromPos);
        if (Physics.Raycast(ray, out var hit, _maxTargetingDistance, _attackMask))
        {
            return hit.collider.TryGetComponent<PlayerStats>(out _);
        }
        return false;
    }

    protected override void Update()
    {
        if (_isDie)
            return;

        base.Update();
        
        if(_checkPlayerDistTimer > 0)
        {
            _checkPlayerDistTimer -= Time.deltaTime;
        }
        else
        {
            _checkPlayerDistTimer = CHECK_PLAYER_DIST_COOLDOWN;

            float dist = Vector3.Distance(transform.position, Target.Transform.position);
            if(dist <= _maxTargetingDistance && dist >= _minTargetingDistance && CanSeeTarget())
            {
                if (!_isTargeting)
                    StartTargeting();
            }
            else if(_isTargeting || dist < _minTargetingDistance)
            {
                StopTargeting();
            }
            else
            {
                dist = Vector3.Distance(transform.position, _lastBadPose);
                if(dist <= 1)
                {
                    StopTargeting();
                }
                else
                {
                    _lastBadPose = transform.position;
                }
            }
        }

        if(_isTargeting)
        {
            if(_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime;
                if(_attackTimer <= _beforeAttackNotificationTime && !_isPreFire)
                {
                    PreFire();
                }
                else if(!_isPreFire)
                {
                    transform.LookAt(Target.Transform.position);
                }
            }
            else
            {
                _attackTimer = _attackCooldown;
                Fire();
                _comboOfArrows++;
                if(_comboOfArrows >= COUNT_OF_ARROWS_ON_ONE_POINT)
                {
                    _comboOfArrows = 0;
                    StopTargeting();
                    _checkPlayerDistTimer = 5;
                }
            }
        }
        else if(!_onAnimation)
        {
            var diff = (transform.position - _prevPos).magnitude;
            _prevPos = transform.position;
            if(diff / Time.deltaTime < 1)
            {
                _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Idle);
            }
            else
            {
                _animator.SetInteger(STATE, (int)ArcherAnimatorStates.Run);
            }
        }
    }
}
