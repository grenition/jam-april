using System.Collections;
using UnityEngine;

public class WolfAI : BaseEnemy
{
    private enum State { Waiting, RunningAway, RunningForAttack, Attack }

    [SerializeField] private float _runSpeed, _sprintSpeed;
    [SerializeField] private GoToPointPattern _goToPointPattern;
    [SerializeField] private FindNearestTargetPattern _findNearestTargetPattern;
    [SerializeField] private LayerMask _raycastLayerMask;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _damage, _heavyDamage;
    [SerializeField] private SplashEffectSystem _splashEffectSystem;

    [SerializeField] private float _maxDistance, _minDistance;

    public const string STATE = "State";
    private State _state = State.Waiting;

    private Coroutine _loopCor;
    private Vector3 _prevPos = Vector3.zero;

    private Vector3 _preFireDirection = Vector3.zero;
    private bool _isMovingPreFire = false;
    private float _walkSpeed;

    private Vector3 _lookAtPoint;
    private bool _smoothLookAtPoint = false, _detectAttack = false;
    private bool _isParried = false;
    private AttackData _currentAttack;

    private void Start()
    {
        _walkSpeed = _speed;
        _lookAtPoint = Target.Transform.position;
        _loopCor = StartCoroutine(LoopIE());
        _goToPointPattern.Initialize(this);
        _findNearestTargetPattern.Initialize(this);
        _findNearestTargetPattern.PossibleTargets.Add(Target.Transform.gameObject);
        _findNearestTargetPattern.StartPattern();
    }

    public override void Hurt(GameObject source, AttackData data)
    {
        if(_findNearestTargetPattern.Working)
        {
            foreach(var target in _findNearestTargetPattern.PossibleTargets)
            {
                if(target == source)
                {
                    TrySetTarget(target);
                    break;
                }
            }
            _findNearestTargetPattern.StopPattern();
        }

        if(_loopCor != null)
        {
            StopCoroutine(_loopCor);
        }
        _loopCor = StartCoroutine(LoopIE());
        _animator.SetInteger(STATE, (int)WolfAnimatorStates.Hurt);

        if (_isParried)
            data.Damage *= 2;

        _splashEffectSystem.PlayEffect();

        base.Hurt(source, data);
    }

    private bool CanSeeTargetFrom(Vector3 fromPos)
    {
        var ray = new Ray(fromPos, Target.Transform.position - fromPos);
        if (Physics.Raycast(ray, out var hit, _maxDistance, _raycastLayerMask))
        {
            return hit.collider.TryGetComponent<PlayerStats>(out _);
        }
        return false;
    }

    private void FindGoodPos(Vector3 initDirection, float length)
    {
        //var initDirection = (transform.position - Target.Transform.position).normalized;
        for (int attempts = 0; attempts < 15; attempts++)
        {
            float range = 60;
            if (attempts > 4)
                range = 90;
            else if (attempts > 6)
                range = 130;
            else if (attempts > 9)
                range = 180;

            float newAngle = Random.Range(-range, range) * Mathf.Deg2Rad;
            var direction = new Vector3(
                initDirection.x * Mathf.Cos(newAngle) - initDirection.z * Mathf.Sin(newAngle),
                0,
                initDirection.x * Mathf.Sin(newAngle) + initDirection.z * Mathf.Cos(newAngle));
            var pos = Target.Transform.position + direction * length;

            if (CanSeeTargetFrom(pos) && _goToPointPattern.SetPoint(pos))
            {
                _goToPointPattern.StartPattern();
                return;
            }
        }
    }

    private IEnumerator LoopIE()
    {
        yield return new WaitForSeconds(.5f);
        _animator.SetInteger(STATE, (int)WolfAnimatorStates.Idle);

        while(true)
        {
            _goToPointPattern.StartPattern();
            var initDirection = (transform.position - Target.Transform.position).normalized;
            FindGoodPos(initDirection, (_minDistance + _maxDistance) / 2);

            while (true)
            {
                _state = State.Waiting;
                float dist = Vector3.Distance(transform.position, Target.Transform.position);
                if (dist >= _minDistance && dist <= _maxDistance && CanSeeTargetFrom(transform.position))
                {
                    break;
                }

                initDirection = (transform.position - Target.Transform.position).normalized;
                FindGoodPos(initDirection, (_minDistance + _maxDistance) / 2);

                yield return new WaitForSeconds(1);
            }

            _goToPointPattern.StopPattern();
            _lookAtPoint = Target.Transform.position;
            _smoothLookAtPoint = true;

            if (Random.Range(0, 5) == 0) //HEAVY
            {
                _preFireDirection = (Target.Transform.position - transform.position).normalized;
                yield return new WaitForSeconds(.6f);
                _smoothLookAtPoint = false;
                transform.LookAt(_lookAtPoint);

                _animator.SetInteger(STATE, (int)WolfAnimatorStates.Run);
                _state = State.RunningForAttack;
                _isMovingPreFire = true;
                _currentAttack = new(_heavyDamage, DamageType.HEAVY_ATTACK,
                    0, AttackColliderType.Slash, 6);
                _detectAttack = true;

                yield return new WaitForSeconds(1);
                _animator.SetInteger(STATE, (int)WolfAnimatorStates.Idle);
            }
            else //QUICK
            {
                _state = State.Attack;
                _animator.SetInteger(STATE, (int)WolfAnimatorStates.Attack);
                _preFireDirection = (Target.Transform.position - transform.position).normalized;

                yield return new WaitForSeconds(1);

                _animator.SetInteger(STATE, (int)WolfAnimatorStates.Idle);

                yield return new WaitForSeconds(1);
            }

            _state = State.RunningAway;
            _speed = _runSpeed;

            _goToPointPattern.StartPattern();

            while (true)
            {
                _state = State.RunningAway;
                float dist = Vector3.Distance(transform.position, Target.Transform.position);
                if (dist >= _maxDistance)
                {
                    break;
                }

                initDirection = (Target.Transform.position - transform.position).normalized;
                FindGoodPos(initDirection, 2 * _maxDistance);

                yield return new WaitForSeconds(1);
            }

            _state = State.Waiting;
            _speed = _walkSpeed;

            yield return new WaitForSeconds(1.8f);
        }
    }

    public void AttackStart()
    {
        _isMovingPreFire = true;
        _smoothLookAtPoint = false;
        transform.LookAt(_lookAtPoint);
        _currentAttack = new(_damage, DamageType.QUICK_ATTACK,
                    0, AttackColliderType.Slash, 3);
        _detectAttack = true;
    }

    public void AttackEnd()
    {
        _isMovingPreFire = false;
    }

    protected override void Update()
    {
        base.Update();

        if(_state == State.Waiting || _state == State.RunningAway)
        {
            var diff = (transform.position - _prevPos).magnitude;
            _prevPos = transform.position;

            WolfAnimatorStates setState = WolfAnimatorStates.Idle;

            if (diff / Time.deltaTime >= 1)
            {
                if (_state == State.Waiting)
                    setState = WolfAnimatorStates.Walk;
                else
                    setState = WolfAnimatorStates.Run;
            }

            _animator.SetInteger(STATE, (int)setState);
        }
        else if(_isMovingPreFire)
        {
            Controller.Move(_preFireDirection *
                (_state == State.Attack ? _sprintSpeed : _runSpeed) * Time.deltaTime);
        }

        if(_smoothLookAtPoint)
        {
            Quaternion preQuat = transform.rotation;
            transform.LookAt(_lookAtPoint);
            transform.rotation = Quaternion.Lerp(preQuat, transform.rotation, Time.deltaTime * 10);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable) && _detectAttack)
        {
            _detectAttack = false;
            if(other.TryGetComponent<ClearBlockUser>(out var user))
            {
                if(user.BlockTimer <= .2f && user.BlockTimer > 0)
                {
                    _isMovingPreFire = false;
                    _state = State.Waiting;
                    _isParried = true;
                    StartCoroutine(ParryIE());
                    return;
                }
            }
            damageable.Hurt(gameObject, _currentAttack);
        }
    }

    private IEnumerator ParryIE()
    {
        yield return new WaitForSeconds(2);
        _isParried = false;
    }
}
