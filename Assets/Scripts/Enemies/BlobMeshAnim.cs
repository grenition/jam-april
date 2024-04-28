using System.Collections;
using System.Collections.Generic;
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
}
