using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerFighting _fighting;

    private void OnEnable()
    {
        ServiceLocator.Register(this);
    }

    public void SetState(PlayerAnimatorState state)
    {
        _animator.SetInteger("State", (int)state);
    }

    public PlayerAnimatorState GetState()
    {
        return (PlayerAnimatorState)_animator.GetInteger("State");
    }

    public void SetHeavyAttack(bool value)
    {
        _animator.SetBool("HeavyAttack", value);
    }

    public void StartBuffer()
    {
        _fighting.StartBuffer();
    }

    public void MoveBeforeAttack(int attackIndex)
    {
        _fighting.MoveBeforeAttack(attackIndex);
    }

    public void Attack(int attackIndex)
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        _fighting.Attack(attackIndex);
    }

    public void CheckBuffer()
    {
        _fighting.CheckBuffer();
    }

    public void EndAttack()
    {
        if(GetState() == PlayerAnimatorState.Idle)
        {
            _fighting.EndAttack();
        }
    }
}

public enum PlayerAnimatorState
{
    Idle, Running, Jump, Attack
}