using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BlobMeshAnim : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private float _speed;

    private float[] _keys = new float[3];

    private void Start()
    {
        for(int i = 0; i < _keys.Length; i++)
        {
            _keys[i] = i;
        }
    }

    private void Update()
    {
        for(int i = 0; i < _keys.Length; ++i)
        {
            _renderer.SetBlendShapeWeight(i, 40 * Mathf.Sin(_keys[i]) + 50);
            _keys[i] += Time.deltaTime * _speed;
        }
    }

    public void DestroyBlob()
    {
        StartCoroutine(DestroyAnimIE());
    }

    private IEnumerator DestroyAnimIE()
    {
        var scale = transform.localScale;
        transform.DOScale(scale * 1.2f, 1);
        yield return new WaitForSeconds(1);
        transform.DOScale(Vector3.zero, 1);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
