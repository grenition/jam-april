using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RedCapAnimtionState
{
    none,
    gathering,
    talking
}

[RequireComponent(typeof(RedCap))]
public class RedCapAnimations : MonoBehaviour
{
    [SerializeField] private float _interpolationFactor = 5f;
    [SerializeField] private Animator _animator;

    private RedCap _redCap;
    private float _curMovement;
    private RedCapAnimtionState _animState;

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

        if(movement > 0.5f)
        {
            _animator.SetBool("Gathering", false);
            _animator.SetBool("Talking", false);
            _animState = RedCapAnimtionState.none;
        }
    }

    public void SetAnimState(RedCapAnimtionState animState)
    {
        _animState = animState;
        switch (_animState)
        {
            case RedCapAnimtionState.none:
                _animator.SetBool("Gathering", false);
                _animator.SetBool("Talking", false);
                break;
            case RedCapAnimtionState.gathering:
                _animator.SetBool("Gathering", true);
                _animator.SetBool("Talking", false);
                break;
            case RedCapAnimtionState.talking:
                _animator.SetBool("Gathering", false);
                _animator.SetBool("Talking", true);
                break;
        }
    }
}
