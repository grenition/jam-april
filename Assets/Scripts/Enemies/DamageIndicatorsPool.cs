using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorsPool : MonoBehaviour
{
    [SerializeField] private DamageIndicator _indicatorPrefab;

    private static List<DamageIndicator> _pool = new();
    private static DamageIndicator _staticPrefab;
    private static int _poolIndex = 0;
    private static Transform _indicatorsParent;

    private void Start()
    {
        _indicatorsParent = transform;
        _staticPrefab = _indicatorPrefab;
        for(int i = 0; i < 5; i++)
        {
            var pool = Instantiate(_indicatorPrefab, _indicatorsParent);
            pool.gameObject.SetActive(false);
            _pool.Add(pool);
        }
    }

    public static void SpawnIndicator(float damage, Vector3 pos, DamageType type)
    {
        if(type == DamageType.HEAL)
        {
            SpawnIndicator(damage, pos, Color.green);
        }
        else if(type == DamageType.QUICK_ATTACK)
        {
            SpawnIndicator(damage, pos, Color.yellow);
        }
        else
        {
            SpawnIndicator(damage, pos, Color.red);
        }
    }

    public static void SpawnIndicator(float damage, Vector3 pos)
    {
        SpawnIndicator(damage, pos, Color.yellow);
    }

    public static void SpawnIndicator(float damage, Vector3 pos, Color color)
    {
        if (_pool[_poolIndex].gameObject.activeSelf)
        {
            _poolIndex = 0;

            var indicator = Instantiate(_staticPrefab, _indicatorsParent);
            _pool.Add(indicator);
            indicator.Initialize(damage, pos, color);
        }
        else
        {
            _pool[_poolIndex].Initialize(damage, pos, color);
            _poolIndex = (_poolIndex + 1) % _pool.Count;
        }
    }
}
