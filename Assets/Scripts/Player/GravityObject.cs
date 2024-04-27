using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] private Transform _floorCollider;
    [SerializeField] private Vector3 _floorColliderSize;
    [SerializeField] private LayerMask _floorColliderMask;

    public const float GRAVITY = 16;

    public bool OnLand { get; private set; }
    public float VelocityY { get; set; }

    private void Update()
    {
        if (Physics.CheckBox(_floorCollider.position, _floorColliderSize / 2,
            Quaternion.identity, _floorColliderMask))
        {
            if (VelocityY <= 0)
            {
                VelocityY = 0;
                OnLand = true;
            }
        }
        else
        {
            OnLand = false;
            VelocityY -= GRAVITY * Time.deltaTime;
        }
    }
}
