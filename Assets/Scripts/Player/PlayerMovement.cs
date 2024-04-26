using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _initSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _jumpHeight, _dashCooldown, _dashLength;
    [SerializeField] private GravityObject _gravityObject;

    private float _dashTimer = 0;

    public const KeyCode JUMP_KEY = KeyCode.Space;
    public const KeyCode DASH_KEY = KeyCode.LeftShift;

    public CharacterController Controller { get; private set; }

    public bool CanMove { get; set; } = true;
    public bool CanJump { get; set; } = true;
    public float Gravity { get { return _gravity; } set { _gravity = value; } }
    public bool OnAnimation { get; set; }
    public Vector3 LastFrameInputVector { get; private set; }
    public GravityObject GravityObject => _gravityObject;

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

        //Jumping
        if(CanJump && Input.GetKeyDown(JUMP_KEY) && _gravityObject.OnLand && !OnAnimation)
        {
            _gravityObject.VelocityY = Mathf.Sqrt(2 * Gravity * _jumpHeight);
        }

        moveVec += Vector3.up * _gravityObject.VelocityY;

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
