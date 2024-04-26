using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _initSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private Transform _floorCollider;
    [SerializeField] private Vector3 _floorColliderSize;
    [SerializeField] private LayerMask _floorColliderMask;
    [SerializeField] private float _jumpHeight, _dashCooldown, _dashLength;

    private Vector3 _velocity = Vector3.zero;
    private float _dashTimer = 0;

    public const KeyCode JUMP_KEY = KeyCode.Space;
    public const KeyCode DASH_KEY = KeyCode.LeftShift;

    public CharacterController Controller { get; private set; }

    public bool CanMove { get; set; } = true;
    public bool CanJump { get; set; } = true;
    public float Gravity { get { return _gravity; } set { _gravity = value; } }
    public bool OnLand { get; private set; }
    public bool OnAnimation { get; set; }
    public Vector3 LastFrameInputVector { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
    }

    public void Dash(Vector3 moveVec)
    {
        if(_dashTimer == 0)
        {
            _dashTimer = _dashCooldown;
            moveVec = moveVec.normalized * _dashLength;
            Controller.Move(moveVec);
        }
    }

    private void Update()
    {
        //WASD Movement
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVec = inputVec * _initSpeed;
        LastFrameInputVector = inputVec;

        if(!CanMove || OnAnimation)
        {
            moveVec = Vector3.zero;
        }
        else if(inputVec.magnitude > 0)
        {
            transform.LookAt(transform.position + inputVec * 10);
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
        if(CanJump && Input.GetKeyDown(JUMP_KEY) && OnLand && !OnAnimation)
        {
            _velocity = Vector3.up * Mathf.Sqrt(2 * Gravity * _jumpHeight);
        }

        moveVec += _velocity;

        Controller.Move(moveVec * Time.deltaTime);

        //Dashing
        if(!OnAnimation && Input.GetKeyDown(DASH_KEY) && inputVec.magnitude > 0)
        {
            Dash(inputVec);
        }

        if(_dashCooldown > 0)
        {
            _dashTimer = Mathf.Clamp(_dashTimer - Time.deltaTime, 0, _dashCooldown);
        }
    }
}
