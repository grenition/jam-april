using System.Collections;
using UnityEngine;

public class BaseMeleeEnemy : BaseEnemy
{
    [SerializeField] private FollowTargetPattern _followTargetPattern;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private LayerMask _attackMask;
    [SerializeField] private float _damage;

    [SerializeField] private GameObject TEST_preAttackNotification;

    private float _attackTimer = 0;

    protected virtual void Start()
    {
        _attackTimer = _attackCooldown;
        _followTargetPattern.Initialize(this);
        _followTargetPattern.StartPattern();
    }

    private void PreAttack()
    {
        _attackTimer = _attackCooldown;
        StartCoroutine(PreAttackIE());
    }

    private IEnumerator PreAttackIE()
    {
        TEST_preAttackNotification.SetActive(true);
        yield return new WaitForSeconds(.5f);
        TEST_preAttackNotification.SetActive(false);
        Attack();
    }

    private void Attack()
    {
        if (IsStuck)
            return;

        _attackTimer = _attackCooldown;
        var direction = (Target.Transform.position - transform.position).normalized;
        var center = transform.position + direction * 3;
        var size = new Vector3(1.5f, 1f, 1.5f);
        var colliders = Physics.OverlapBox(center, size, transform.rotation, _attackMask);
        foreach(var collider in colliders)
        {
            if(collider.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Hurt(gameObject,
                    new(10, DamageType.QUICK_ATTACK, 0, AttackColliderType.Slash, 5));
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if(_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }
        else
        {
            float dist = Vector3.Distance(transform.position, Target.Transform.position);
            if (dist < 2.5f)
            {
                PreAttack();
            }
        }
    }
}
