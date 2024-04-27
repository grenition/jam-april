using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RedCap))]
public class RedCapAnimations : MonoBehaviour
{
    [SerializeField] private float _interpolationFactor = 5f;
    [SerializeField] private Animator _animator;

    private RedCap _redCap;
    private float _curMovement;

    private void Awake()
    {
        _redCap = GetComponent<RedCap>();
    }

    private void Update()
    { 
        float movement = _redCap.Agent.velocity.magnitude / _redCap.Agent.speed;
        movement = Mathf.Clamp01(movement);

        _curMovement = Mathf.Lerp(_curMovement, movement, _interpolationFactor * Time.deltaTime);

        _animator.SetFloat("Movement", _curMovement * 0.5f);
    }
}
