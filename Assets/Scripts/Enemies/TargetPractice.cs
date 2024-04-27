using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPractice : MonoBehaviour, IDamageable
{
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private DamageIndicatorsPool _damageIndicatorsPool;

    private Material _mat;

    private Coroutine _cor;

    private void Awake()
    {
        if(_mesh != null)
        {
            _mat = Instantiate(_mesh.material);
            _mesh.material = _mat;
        }
    }

    public void Hurt(GameObject source, AttackData data)
    {
        _damageIndicatorsPool.SpawnIndicator(data.Damage, data.Type, 1);
        if(_cor != null)
        {
            StopCoroutine(_cor);
        }
        _cor = StartCoroutine(AnimIE());
    }

    private IEnumerator AnimIE()
    {
        _mat.color = Color.red;
        yield return new WaitForSeconds(.5f);
        _mat.color = Color.white;
    }
}
