using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownFollower : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private float _distance = 10f;
    [SerializeField] private Transform _target;

    private Vector3 _position;
    private Quaternion _rotation;
    private Vector3 _startPosition;

    private void OnEnable()
    {
        _position = transform.position;
        _rotation = transform.rotation;
        _startPosition = _target.position;
    }

    private void LateUpdate()
    {
        Vector3 horizontal = Vector3.Lerp(_position, _target.position, _followSpeed * Time.deltaTime);
        horizontal = VectorMathf.RemoveDotVector(horizontal, Vector3.up);

        Vector3 vertical = VectorMathf.ExtractDotVector(_startPosition, Vector3.up) + Vector3.up * _distance;

        _position = horizontal + vertical;

        transform.position = _position;
        transform.rotation = _rotation;
    }
}
