using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _initSpeed;
    [SerializeField] private float _jumpHeight, _dashCooldown, _dashLength;
    [SerializeField] private GravityObject _gravityObject;
    [SerializeField] private float _worldAxisAngle;
    [SerializeField] private LineRenderer _dashRenderer;

    private float _dashTimer = 0;
    private Vector3 _knockbackDirection = Vector3.zero;
    private float _knockbackStrength = 0;

    public const KeyCode JUMP_KEY = KeyCode.Space;
    public const KeyCode DASH_KEY = KeyCode.LeftShift;
    public const float KNOCKBACK_SLOWNESS = 3;

    public CharacterController Controller { get; private set; }

    public bool CanMove { get; set; } = true;
    public bool CanJump { get; set; } = true;
    public bool OnAnimation { get; set; }
    public Vector3 LastFrameInputVector { get; private set; }
    public GravityObject GravityObject => _gravityObject;
    public PlayerStats Stats { get; private set; }
    public PlayerAnimator Animator { get; private set; }

    private void Awake()
    {
        Stats = GetComponent<PlayerStats>();
        Controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Animator = ServiceLocator.Get<PlayerAnimator>();
    }

    public void Dash(Vector3 moveVec)
    {
        if(_dashTimer == 0)
        {
            _dashTimer = _dashCooldown;
            moveVec = moveVec.normalized * _dashLength;
            _dashRenderer.SetPosition(0, transform.position);
            Controller.Move(moveVec);
            _dashRenderer.SetPosition(1, transform.position);
            _dashRenderer.material.color = Color.white;
            StartCoroutine(DashIE());
        }
    }

    private IEnumerator DashIE()
    {
        yield return new WaitForSeconds(.3f);
        _dashRenderer.material.DOColor(new Color(1, 1, 1, 0), .6f);
    }

    public void Knockback(float strength, Vector3 direction)
    {
        _knockbackDirection = direction.normalized;
        _knockbackStrength = strength;
    }

    private Vector3 RotateByY(Vector3 vector, float angle)
    {
        if (vector.x != 0 && vector.z != 0)
            vector *= .7f;
        angle *= Mathf.Deg2Rad;
        return new Vector3(
            vector.x * Mathf.Cos(angle) - vector.z * Mathf.Sin(angle),
            0,
            vector.x * Mathf.Sin(angle) + vector.z * Mathf.Cos(angle)
        );
    }

    private void Update()
    {
        //WASD Movement
        Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        inputVec = RotateByY(inputVec, _worldAxisAngle);
        Vector3 moveVec = inputVec * _initSpeed;
        LastFrameInputVector = inputVec;

        if(!CanMove || OnAnimation || Stats.IsStuck || Stats.Fighting.IsBlocking)
        {
            moveVec = Vector3.zero;
        }
        else if(inputVec.magnitude > 0)
        {
            transform.LookAt(transform.position + inputVec * 10);
            Animator.SetState(PlayerAnimatorState.Running);
        }
        else
        {
            Animator.SetState(Stats.Fighting.IsBlocking ?
                PlayerAnimatorState.Blocking :
                PlayerAnimatorState.Idle);
        }

        //Jumping
        //if(CanJump && Input.GetKeyDown(JUMP_KEY) && _gravityObject.OnLand && !OnAnimation && !Stats.IsStuck)
        //{
        //    _gravityObject.VelocityY = Mathf.Sqrt(2 * GravityObject.GRAVITY * _jumpHeight);
        //    Animator.SetState(PlayerAnimatorState.Jump);
        //}

        moveVec += Vector3.up * _gravityObject.VelocityY;

        //Knockback
        if(_knockbackStrength > 0)
        {
            moveVec += _knockbackDirection * _knockbackStrength;
            _knockbackStrength = Mathf.Clamp(_knockbackStrength - KNOCKBACK_SLOWNESS * Time.deltaTime,
                0, 100);
        }

        Controller.Move(moveVec * Time.deltaTime);

        //Dashing
        if(!OnAnimation && Input.GetKeyDown(DASH_KEY) && inputVec.magnitude > 0 && !Stats.IsStuck)
        {
            Dash(inputVec);
        }

        if(_dashCooldown > 0)
        {
            _dashTimer = Mathf.Clamp(_dashTimer - Time.deltaTime, 0, _dashCooldown);
        }
    }
}
