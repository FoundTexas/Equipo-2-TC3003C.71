using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement3D : MonoBehaviour
{
    [Header("Horizontal movement")]
    // Variables needed for character movement and camera functionality
    public float moveSpeed = 15f;
    public float maxSpeed = 20f;
    private float originalSpeed;
    private Vector3 direction;
    private Vector3 moveDirection;
    new private Rigidbody rigidbody;
    private bool canMove = true;

    [Header("Camera movement")]
    // Variables needed for camera turning
    new public Transform camera;
    public float turnTime = 0.1f;
    private float turnVelocity;

    [Header("Slope Movement")]
    // Variables needed for movement while standing on a slope
    public float maxSlopeAngle = 60f;
    public float onSlopeSpeed = 5f;
    private RaycastHit slopeHit;

    [Header("Jumping")]
    // Variables needed for gravity functionality
    public float jumpHeight = 12f;
    [Tooltip("Value determining how fast the player will fall")]
    public float fallMultiplier = 2.5f;
    [Tooltip("Value determining how far will the long jump go")]
    public float lowJumpMultiplier = 4f;
    private float gravity =  -9.81f;
    private Vector3 velocity;

    [Header("Diving")]
    public float diveForce = 5f;
    public float airTimeWait = 0.2f;
    public float canDiveStart = 0.2f;
    private bool canDive = false;

    [Header("Wall jumping")]
    public float wallDistance = 0.7f;
    public float minJumpHeight = 0.75f;
    public float wallJumpForce = 7f;
    public float wallJumpSideForce = 12f;
    public float exitWallTime = 0.7f;
    private bool wallFound = false;
    public LayerMask wallMask;
    private RaycastHit wallHit;

    [Header("Ground controller")]
    // Variables needed to check ground below player
    public float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    private bool isGrounded;

    [Header("Crouching")]
    // Variables needed for crouching
    public float crouchSpeed = 3.5f;
    public float crouchHeight = 0.5f;
    private float originalHeight;

    [Header("Dashing")]
    public float dashForce = 3f;
    public float doubleTapTime = 0.4f;
    private float doubleTapDelta;
    private bool doubleTap = false;

    [Header("Keyboard inputs")]
    public KeyCode jumpInput = KeyCode.Space;
    public KeyCode crouchInput = KeyCode.LeftShift;

    AudioAndVideoManager anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<AudioAndVideoManager>();
        rigidbody = GetComponent<Rigidbody>();
        originalHeight = transform.localScale.y;
        originalSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SendAnimationVals();
        CheckGrounded();
        CheckInputs();
        CheckWallJump();
        CheckMove();
        CheckDash();
        SpeedControl();
        CheckCrouch();
        CheckJump();
        CheckDive();
    }

    void SendAnimationVals(){
        anim.IsOnGround(isGrounded);
        anim.SetIfMovement(rigidbody.velocity.magnitude);
        anim.IsOnWall(wallFound);
    }

    private void CheckGrounded()
    {
        //Check if object is grounded by creating an invisible sphere
        //and checking if anything contained in groundMask is in contact with it
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        if(isGrounded)
            canDive = false;
    }

    private void CheckInputs()
    {
        //Gather Keyboard Input and create resulting vector
        //Normalized to avoid faster movement in diagonals
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = 0f;
        if(!wallFound || isGrounded)
            verticalInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    private void CheckJump()
    {
        //Gravity Control
        if(Input.GetKeyDown(jumpInput) && isGrounded)
        {
            StartCoroutine(EnableDive());
            anim.jumpSound();
            rigidbody.velocity = new Vector3(velocity.x, 0f, velocity.z);
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }

        // Manage Long/short jump
        if(rigidbody.velocity.y < 0)
            rigidbody.velocity += Vector3.up * gravity * (fallMultiplier - 1) * Time.deltaTime;
        else if(rigidbody.velocity.y > 0 && !Input.GetKey(jumpInput))
            rigidbody.velocity += Vector3.up * gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    private void CheckMove()
    {
        if(direction.magnitude >= 0.1f)
        {
            //Utilize Atan2 function to find angle player should look at based on direction vector and camera angle
            //Utilize SmoothDampAngle function to change the angle based on established variables for a smoother look
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float resultAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, resultAngle, 0f);
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rigidbody.useGravity = !OnSlope();

            if(!canMove)
                return;
            
            if(OnSlope())
            {
                rigidbody.AddForce(Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized * moveSpeed * onSlopeSpeed, ForceMode.Force);
                if(rigidbody.velocity.y > 0)
                    rigidbody.AddForce(Vector3.down * 10f, ForceMode.Force);
            }
            else
                rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, 
        out slopeHit, groundCheck.position.y + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
    private void SpeedControl()
    {
        if(OnSlope())
        {
            if(rigidbody.velocity.magnitude > moveSpeed)
                rigidbody.velocity = rigidbody.velocity.normalized * moveSpeed;
        }
        else
        {
            rigidbody.velocity = new Vector3(
                Mathf.Clamp(rigidbody.velocity.x, -maxSpeed, maxSpeed),
                Mathf.Clamp(rigidbody.velocity.y, -maxSpeed, maxSpeed),
                Mathf.Clamp(rigidbody.velocity.z, -maxSpeed, maxSpeed)    
                );
            /*
            Vector3 currentVelocity = new Vector3(  rigidbody.velocity.x, 
                                                    0f, 
                                                    rigidbody.velocity.z);

            if(currentVelocity.magnitude >= maxSpeed)
            {
                Vector3 limitedVelocity = currentVelocity.normalized * maxSpeed;
                rigidbody.velocity = new Vector3(   limitedVelocity.x, 
                                                    rigidbody.velocity.y,
                                                    limitedVelocity.z);
            }*/
        }
    }

    private void CheckCrouch()
    {
        if(Input.GetKeyDown(crouchInput))
        {
            transform.localScale = new Vector3( transform.localScale.x,
                                                crouchHeight,
                                                transform.localScale.z);
            rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            moveSpeed = crouchSpeed;
        }

        if(Input.GetKeyUp(crouchInput))
        {
            transform.localScale = new Vector3( transform.localScale.x,
                                                originalHeight,
                                                transform.localScale.z);
            moveSpeed = originalSpeed;
        }
    }

    private void CheckWallJump()
    {
        wallFound = Physics.Raycast(transform.position, transform.forward, out wallHit, wallDistance, wallMask);

        if(wallFound && AboveGround())
        {
            if(Input.GetKeyDown(jumpInput))
                WallJump();
        }
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, groundMask);
    }

    private void WallJump()
    {
        if(!canMove)
            return;
        
        Vector3 wallNormal = wallHit.normal;

        Vector3 jumpForce = transform.up * wallJumpForce + wallNormal * wallJumpSideForce;

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
        rigidbody.AddForce(jumpForce, ForceMode.Impulse);

        transform.Rotate(new Vector3(0, 180, 0));

        StartCoroutine(ResetWallJump());
    }

    private IEnumerator ResetWallJump()
    {
        canMove = false;
        yield return new WaitForSeconds(exitWallTime);
        canMove = true; 
    }
    private IEnumerator EnableDive()
    {
        yield return new WaitForSeconds(canDiveStart);
        canDive = true; 
    }

    private void CheckDive()
    {
        if(canDive && Input.GetKeyDown(jumpInput) && !wallFound)
        {
            anim.DiveSound();
            StartCoroutine(Dive());
        }
    }

    private IEnumerator Dive()
    {
        canMove = false;
        canDive = false;
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(airTimeWait);
        rigidbody.AddForce( this.transform.forward * diveForce + Vector3.up * (jumpHeight / 2), ForceMode.Impulse);// new Vector3(moveDirection.x  * diveForce, (jumpHeight/3)  * diveForce , moveDirection.z * diveForce), ForceMode.Impulse);
        rigidbody.useGravity = true;
        yield return new WaitForSeconds(airTimeWait / 2);
        canMove = true;
    }

    private void CheckDash()
    {
        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && doubleTap) {
            if(Time.time - doubleTapDelta < doubleTapTime) {
                Dash();
                print("Dashing");
                doubleTapDelta = 0f;
            }
            doubleTap = false;
        }

        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !doubleTap) {
            doubleTap = true;
            doubleTapDelta = Time.time;
        }
    }

    private void Dash()
    {
        Vector3 forceToApply = transform.forward * dashForce;
        rigidbody.AddForce(rigidbody.velocity * dashForce, ForceMode.Impulse);
    }
}
