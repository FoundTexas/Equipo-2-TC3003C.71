using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    Player PlayerInput;
    InputAction MoveValue;
    InputAction JumpInput,JumpInputHold;

    [Header("Camera movement")]
    // Variables needed for camera turning
    public float turnTime = 0.1f;
    private float turnVelocity;
    public Transform cam;
    float targetAngle;
    float resultAngle;


    [Tooltip("Maximum speed of the object")]
    [SerializeField] float speed;
    [Tooltip("amount of incement per frame")]
    [SerializeField] float speedStep;
    float curSpeed;


    [Tooltip("Player's Jumping Force")]
    [SerializeField] float jumpForce;
    [Tooltip("Player's jump time")]
    [SerializeField] float jumpTime = 2f;
    [Tooltip("Player's gravity")]
    [SerializeField] float gravity = 9.81f;
    public GameObject jumpParticles;
    public float gravityModifier = 1;
    float curJumpTime = 0f;


    [Header("Wall jumping")]
    public float wallDistance = 0.7f;
    public float minJumpHeight = 0.75f;
    public float wallJumpForce = 7f;
    public float wallJumpSideForce = 12f;
    public float exitWallTime = 0.7f;
    public LayerMask wallMask;
    private RaycastHit wallHit;
    bool wallFound;

    bool canMove = true;
    CharacterController controller;
    Vector3 movDirection;
    private AudioAndVideoManager anim;

    // ------------------------------------- Unity Methods

    private void Awake() {
        PlayerInput = new Player();
    }
    private void OnEnable() {
        MoveValue = PlayerInput.Game.WASD;
        MoveValue.Enable();
        JumpInput = PlayerInput.Game.Jump;
        JumpInput.Enable();
    }
    private void OnDisable() {
        MoveValue.Disable();
        JumpInput.Disable();
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<AudioAndVideoManager>();
        cam = Camera.main.transform;
        jumpParticles.SetActive(false);
    }
    void Update()
    {
        movDirection = new Vector3(
            movDirection.x,
            movDirection.y,
            movDirection.z);
        Jump();
        Aim();
        WASD();

        if(!controller.isGrounded){
            movDirection.y = movDirection.y - gravity * gravityModifier * Time.deltaTime;
        }
        movDirection.y = Mathf.Clamp(movDirection.y, -gravity * gravityModifier * 2, jumpForce * 100);

        controller.Move(movDirection * Time.deltaTime);
        SendAnimationVals();

    }

    void SendAnimationVals()
    {
        anim.IsOnGround(controller.isGrounded);
        anim.SetIfMovement(Mathf.Abs( MoveValue.ReadValue<Vector2>().x) + Mathf.Abs( MoveValue.ReadValue<Vector2>().y));
        anim.IsOnWall(wallFound);
    }

    void SetTargetAngle(Vector2 mov)
    {
        targetAngle = Mathf.Atan2(mov.x, mov.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        resultAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, mov.magnitude == 0 ? 0 : turnTime);
        transform.rotation = Quaternion.Euler(0f, resultAngle, 0f);
    }

    void Aim()
    {
        if (Input.GetMouseButton(1)) { SetTargetAngle(Vector2.zero); }
    }

    void WASD()
    {
        if (!canMove){
            curSpeed = 0;
            return;
        }

        Vector2 mov = MoveValue.ReadValue<Vector2>();//new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (mov.magnitude > 0.3f)
        {
            SetTargetAngle(mov);
        }
        if (mov.magnitude != 0)
        {
            if (curSpeed < speed)
            {
                curSpeed += speedStep;
            }
        }
        else if (controller.isGrounded)
        {
            if (curSpeed > 0)
            {
                curSpeed -= speedStep*3;
            }
            else if (curSpeed <= 0)
            {
                curSpeed = 0;
            }
        }

        movDirection = Quaternion.Euler(0f, targetAngle, 0f) * new Vector3(
            0,
            movDirection.y,
            1);
        movDirection = new Vector3(
            movDirection.x * curSpeed,
            movDirection.y,
            movDirection.z * curSpeed);

    }
    void Jump()
    {
        Debug.Log("Jump");
        gravityModifier = 1;

        if (!canMove)
            return;


        anim.SetIfMovement(1);
        wallFound = Physics.Raycast(transform.position, transform.forward, out wallHit, 1, wallMask);

        if (wallFound && !controller.isGrounded)
        {
            if (JumpInput.IsPressed())
            {
                WallJump();
            }
        }

        if (controller.isGrounded)
        {
            jumpParticles.SetActive(false);
            curJumpTime = 0;
            if (JumpInput.IsPressed())
            {
                movDirection.y = jumpForce;
                jumpParticles.SetActive(true);
            }
        }
        else
        {
            if (JumpInput.ReadValue<float>() > 0.1f)
            {
                jumpParticles.SetActive(true);
                if (curJumpTime < jumpTime)
                {
                    gravityModifier = 0.5f;
                    curJumpTime += Time.deltaTime;
                }
                else
                {
                    gravityModifier = 1f;
                }

            }
            if (!JumpInput.IsPressed())
            {
                gravityModifier = 1f;
                jumpParticles.SetActive(false);
            }
        }

        Debug.Log(JumpInput.ReadValue<float>());
    }
    private void WallJump()
    {
        if (!canMove)
            return;

        Vector3 wallNormal = wallHit.normal;
        transform.Rotate(new Vector3(0, 180, 0));

        Vector3 jumpDir = transform.up * wallJumpForce + wallNormal * wallJumpSideForce;

        movDirection = jumpDir;

        StartCoroutine(ResetWallJump());
    }
    private IEnumerator ResetWallJump()
    {
        canMove = false;
        yield return new WaitForSeconds(exitWallTime);
        canMove = true;
    }
}
