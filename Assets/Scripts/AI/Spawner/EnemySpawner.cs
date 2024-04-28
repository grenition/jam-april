using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemySpawner : MonoBehaviour
{
    public IReadOnlyList<BaseEnemy> Enemies => _enemies;
    public bool IsWorking {  get; private set; }  

    [SerializeField] private BaseEnemy[] _enemyPrefabs;
    [SerializeField] private float _spawnCooldown = 2f;
    [SerializeField] private int _maxEnemiesCount = 6;
    [SerializeField] private Transform[] _spawnPoints;

    private List<BaseEnemy> _enemies = new();

    public void StartSpawner()
    {
        StartCoroutine(SpawnCoroutine());
    }
    public void StopSpawner()
    {
        StopAllCoroutines();
        IsWorking = false;
    }
    private IEnumerator SpawnCoroutine()
    {
        if (IsWorking)
            yield break;

        IsWorking = true;

        while (true)
        {
            if (_enemies.Count >= _maxEnemiesCount)
            {
                yield return null;
                continue;
            }

            Vector3 point = _spawnPoints[Random.Range(0, _spawnPoints.Length)].position;

            var enemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)],
                point, Quaternion.identity);

            _enemies.Add(enemy);

            enemy.OnDestroyEvent.Bind(() =>
            {
                if(_enemies.Contains(enemy))
                    _enemies.Remove(enemy);
            }).AddTo(this);

            yield return new WaitForSeconds(_spawnCooldown);
        }

    }
}
