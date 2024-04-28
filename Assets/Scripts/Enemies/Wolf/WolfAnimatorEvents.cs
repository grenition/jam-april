using UnityEngine;

public class WolfAnimatorEvents : MonoBehaviour
{
    [SerializeField] private WolfAI _wolf;

    public void StartAttack()
    {
        _wolf.AttackStart();
    }

    public void StopAttack()
    {
        _wolf.AttackEnd();
    }
}
