using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;

    private float _curHealth;

    private void Start()
    {
        _curHealth = _maxHealth;
    }

    public void Hurt(GameObject source, DamageType type, float damage)
    {
        _curHealth -= damage;
        if(_curHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
