using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RedCap : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private DamageIndicatorsPool _pool;

    public static RedCap Instance { get; private set; }
    public NavMeshAgent Agent => _agent;
    public event Action Died;

    
    private NavMeshAgent _agent;
    private float _curHealth;
    
    private void Awake()
    {
        Instance = this;
        _agent = GetComponent<NavMeshAgent>();
        _pool.Initialize(_maxHealth);
        _curHealth = _maxHealth;
    }

    public void Hurt(GameObject source, AttackData data)
    {
        _curHealth -= data.Damage;

        _pool.SpawnIndicator(data.Damage, data.Type, _curHealth);

        if(_curHealth <= 0)
        {
            Died?.Invoke();
            var game = ServiceLocator.Get<GameLifetime>();
            game?.Loose(LooseReason.redCapDie);
            Destroy(gameObject);
        }
    }
}
