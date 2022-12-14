using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameManagement;
using Player;
using PlanetCrashUI;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public class Move : MonoBehaviour
{
    PlayerInputs PlayerInput;
    InputAction MoveValue, JumpInput, AimInput, CrouchInput;

    public GameObject playerOnlinePrefab;
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
    float curSpeed, SeedMod;


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

    public float wallDistance = 1.5f;
    public float minJumpHeight = 0.75f;
    public float wallJumpForce = 7f;
    public float wallJumpSideForce = 12f;
    public float exitWallTime = 0.7f;
    public LayerMask wallMask;
    private RaycastHit wallHit;
    bool wallFound;

    [Header("Crouching")]
    [Min(0)] public float crouchSpeed = 3.5f;
    [Tooltip("Height the player will have when crouching")]
    [Min(0)] public float crouchHeight = 0.5f;
    private float originalHeight = 1;
    private RaycastHit upHit;
    bool crouching = false;

    public bool canMove = true;
    public bool wasGrounded = false;
    public bool PossibleDialogue = false;
    CharacterController controller;
    Vector3 movDirection;
    PhotonView view;
    private AudioAndVideoManager anim;

    // ------------------------------------- Unity Methods

    private void Awake()
    {
        PlayerInput = new PlayerInputs();
    }
    private void OnEnable()
    {
        view = GetComponent<PhotonView>();
        if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
        {
            PhotonNetwork.EnableCloseConnection = true;
            MoveValue = PlayerInput.Game.WASD;
            MoveValue.Enable();
            JumpInput = PlayerInput.Game.Jump;
            JumpInput.Enable();
            AimInput = PlayerInput.Game.Aim;
            AimInput.Enable();
            CrouchInput = PlayerInput.Game.Crouch;
            CrouchInput.Enable();

            JumpInput.performed += Jump;
            CrouchInput.performed += CheckCrouch;
        }
    }
    private void OnDisable()
    {
        view = GetComponent<PhotonView>();
        if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
        {
            MoveValue.Disable();
            JumpInput.Disable();
            AimInput.Disable();
            CrouchInput.Disable();
        }
    }
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
        {
            controller = GetComponent<CharacterController>();
            anim = GetComponent<AudioAndVideoManager>();
            
            cam = Camera.main.transform;
            jumpParticles.SetActive(false);
            SeedMod = speed;
            originalHeight = controller.height; //transform.localScale.y;
            FindObjectOfType<MiniMap>().player = this.gameObject;

            StartCoroutine(SetFirstPos());
        }
    }
    // public void CreateOnlinePlayer()
    // {
    //     GameObject player = this.gameObject;
    //     if(GameManager.isOnline && !view)
    //     {
    //         player = PhotonNetwork.Instantiate(playerOnlinePrefab.name, transform.position, Quaternion.identity);
    //         FindObjectOfType<FollowPlayer>().Player = player.transform;
    //         FindObjectOfType<MiniMap>().player = player.gameObject;
    //         Destroy(gameObject);
    //     }
    //     player.GetComponent<Move>().canMove = true;
    // }

    public void setCam(SceneManagement sm)
    {
        view = GetComponent<PhotonView>();
        if (!GameManager.isOnline || GameManager.isOnline  && view.IsMine)
        {
            sm.SetCam(this.transform);
        }
    }

    IEnumerator SetFirstPos()
    {
        yield return new WaitForEndOfFrame();
        transform.position = GameManager.getCheckpoint();
    }
    private void LateUpdate()
    {
        if (!GameManager.isOnline || GameManager.isOnline && view.IsMine)
        {

            SendAnimationVals();
            wallFound = Physics.Raycast(transform.position, transform.forward, out wallHit, wallDistance, wallMask);
            Physics.Raycast(transform.position, transform.up, out upHit, 1.6f, wallMask);

            movDirection = new Vector3(
                movDirection.x,
                movDirection.y,
                movDirection.z);
            JumpHold();
            Aim();
            WASD();

            if (!controller.isGrounded)
            {
                movDirection.y = movDirection.y - gravity * gravityModifier * Time.deltaTime;
            }
            movDirection.y = Mathf.Clamp(movDirection.y, -gravity * gravityModifier * 2, jumpForce * 100);

            controller.Move(movDirection * Time.deltaTime);

            wasGrounded = controller.isGrounded;
            ResetMovement();
        }
    }

    void SendAnimationVals()
    {
        anim.IsOnGround(controller.isGrounded);
        anim.SendCrouching(crouching);

        if (canMove)
        {
            anim.SetIfMovement(Mathf.Abs(MoveValue.ReadValue<Vector2>().x) + Mathf.Abs(MoveValue.ReadValue<Vector2>().y));
        }
        else if (!canMove)
        {
            anim.SetIfMovement(0.01f);
        }
        anim.IsOnWall(wallFound);
    }

    public void SetTargetAngle(Vector2 mov)
    {
        targetAngle = Mathf.Atan2(mov.x, mov.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        resultAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, mov.magnitude == 0 ? 0 : turnTime);
        transform.rotation = Quaternion.Euler(0f, resultAngle, 0f);
    }

    void Aim()
    {
        if (AimInput.ReadValue<float>() > 0.1f) { SetTargetAngle(Vector2.zero); }
    }

    void WASD()
    {
        if (!canMove)
        {
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
            if (curSpeed < SeedMod)
            {
                curSpeed += speedStep;
            }
        }
        else if (controller.isGrounded)
        {
            if (curSpeed > 0)
            {
                curSpeed -= speedStep * 3;
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
    void Jump(InputAction.CallbackContext context)
    {
        if (!canMove || PossibleDialogue)
            return;
        if (wallFound && !controller.isGrounded)
        {
            if (context.performed)
            {
                anim.SetIfMovement(1);
                WallJump();
            }
        }

        if (controller.isGrounded)
        {
            jumpParticles.SetActive(false);
            curJumpTime = 0;
            if (context.performed)
            {
                if (crouching)
                {
                    crouchtoggle();
                }
                anim.SetIfMovement(1);
                movDirection.y = jumpForce;
                jumpParticles.SetActive(true);
            }
        }
    }

    void JumpHold()
    {
        gravityModifier = 1;

        if (!canMove)
            return;

        if (!controller.isGrounded)
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
    }
    void WallJump()
    {
        if (!canMove)
            return;

        Vector3 wallNormal = wallHit.normal;
        transform.Rotate(new Vector3(0, 180, 0));

        Vector3 jumpDir = transform.up * wallJumpForce + wallNormal * wallJumpSideForce;

        movDirection = jumpDir;

        StartCoroutine(ResetWallJump());
    }
    /// <summary>
    /// This method checks if the player can crouch when holding crouch key.
    /// </summary>
    void CheckCrouch(InputAction.CallbackContext context)
    {
        crouchtoggle();
    }

    void crouchtoggle()
    {
        Physics.Raycast(transform.position, transform.up, out upHit, 2f, wallMask);

        Debug.DrawRay(transform.position, transform.up, upHit.transform? Color.green : Color.red, 1);

        Debug.Log(upHit);

        curSpeed = 0;
        crouching = !crouching;

        if (!controller.isGrounded)
        {
            crouching = false;
        }

        if (crouching)
        {
            controller.height = crouchHeight;
            //transform.localScale = new Vector3(transform.localScale.x,
            //                                  crouchHeight,
            //                                transform.localScale.z);
            SeedMod = crouchSpeed;
        }
        else if (!crouching)
        {
            if (upHit.transform)
            {
                SeedMod = crouchSpeed;
                crouching = true;
            }
            else
            {
                SeedMod = speed;
                controller.height = originalHeight;
                //transform.localScale = new Vector3(transform.localScale.x,
                //                                  originalHeight,
                //                                transform.localScale.z);
            }
        }
    }

    public void StopMove()
    {
        movDirection = Quaternion.Euler(0f, targetAngle, 0f) * new Vector3(
           0,
           movDirection.y,
           1);
        movDirection = new Vector3(
            movDirection.x * 0,
            movDirection.y,
            movDirection.z * 0);
    }
    void ResetMovement()
    {
        if (!canMove)
        {
            if (wallFound)
            {
                Debug.Log("Reset");
                //canMove = true;
            }
        }
    }
    private IEnumerator ResetWallJump()
    {
        canMove = false;
        yield return new WaitForSeconds(exitWallTime);
        canMove = true;
    }

    public void AddForce(float force, Vector3 dir, float time)
    {
        movDirection = dir * force;
        StartCoroutine(ForceRoutine(time));
    }
    IEnumerator ForceRoutine(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public void Respawned()
    {
        StopAllCoroutines();
        canMove = true;
    }
}
