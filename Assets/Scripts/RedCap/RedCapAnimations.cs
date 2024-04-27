using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RedCapAnimationState
{
    none,
    gathering,
    talking,
    terrified
}

[RequireComponent(typeof(RedCap))]
public class RedCapAnimations : MonoBehaviour
{
    [SerializeField] private float _interpolationFactor = 5f;
    [SerializeField] private Animator _animator;

    private RedCap _redCap;
    private float _curMovement;
    private RedCapAnimationState _animState;

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

        //if(movement > 0.5f)
        //{
        //    _animator.SetBool("Gathering", false);
        //    _animator.SetBool("Talking", false);
        //    _animator.SetBool("Terrified", false);
        //    _animState = RedCapAnimationState.none;
        //}
    }

    public void SetAnimState(RedCapAnimationState animState)
    {
        _animator.SetBool("Gathering", false);
        _animator.SetBool("Talking", false);
        _animator.SetBool("Terrified", false);
        _animState = animState;
        switch (_animState)
        {
            case RedCapAnimationState.none:
                break;
            case RedCapAnimationState.gathering:
                _animator.SetBool("Gathering", true);
                break;
            case RedCapAnimationState.talking:
                _animator.SetBool("Talking", true);
                break;
            case RedCapAnimationState.terrified:
                _animator.SetBool("Terrified", true);
                break;
        }
    }
}
