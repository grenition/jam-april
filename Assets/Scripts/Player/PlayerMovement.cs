using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _initSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private Transform _floorCollider;
    [SerializeField] private Vector3 _floorColliderSize;
    [SerializeField] private LayerMask _floorColliderMask;
    [SerializeField] private float _jumpHeight;

    private CharacterController _controller;
    private Vector3 _velocity = Vector3.zero;
    public const KeyCode JUMP_KEY = KeyCode.Space;

    public bool CanMove { get; set; } = true;
    public bool CanJump { get; set; } = true;
    public bool OnFight { get; set; } = false;
    public float Gravity { get { return _gravity; } set { _gravity = value; } }
    public bool OnLand { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //WASD Movement
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveVec *= _initSpeed;

        if(!CanMove)
        {
            moveVec = Vector3.zero;
        }

        //Gravity
        if(Physics.CheckBox(_floorCollider.position, _floorColliderSize / 2,
            Quaternion.identity, _floorColliderMask))
        {
            if(_velocity.y < 0)
            {
                _velocity = Vector3.zero;
                OnLand = true;
            }
        }
        else
        {
            OnLand = false;
            _velocity += Vector3.down * Gravity * Time.deltaTime;
        }

        //Jumping
        if(CanJump && Input.GetKeyDown(JUMP_KEY) && !OnFight && OnLand)
        {
            _velocity = Vector3.up * Mathf.Sqrt(2 * Gravity * _jumpHeight);
        }

        moveVec += _velocity;

        _controller.Move(moveVec * Time.deltaTime);
    }
}
