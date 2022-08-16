using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement3D : MonoBehaviour
{
    [Header("Horizontal movement")]
    // Variables needed for character movement and camera functionality
    public float speed = 6f;
    private Vector3 direction;
    new private Rigidbody rigidbody;

    [Header("Camera movement")]
    // Variables needed for camera turning
    new public Transform camera;
    public float turnTime = 0.1f;
    public float turnVelocity;

    [Header("Vertical movement")]
    // Variables needed for gravity functionality
    public float jumpHeight = 8f;
    [Tooltip("Value determining how fast the player will fall")]
    public float fallMultiplier = 2.5f;
    [Tooltip("Value determining how far will the long jump go")]
    public float lowJumpMultiplier = 2f;
    private float gravity =  -9.81f;
    private Vector3 velocity;


    [Header("Ground controller")]
    // Variables needed to check ground below player
    public float groundDistance = 0.4f;
    public Transform groundCheck;
    public LayerMask groundMask;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        CheckInputs();
        CheckMove();
        SpeedControl();
        CheckJump();
    }

    private void CheckGrounded()
    {
        //Check if object is grounded by creating an invisible sphere
        //and checking if anything contained in groundMask is in contact with it
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;
    }

    private void CheckInputs()
    {
        //Gather Keyboard Input and create resulting vector
        //Normalized to avoid faster movement in diagonals
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;
    }

    private void CheckJump()
    {
        //Gravity Control
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody.velocity = new Vector3(velocity.x, 0f, velocity.z);
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }

        // Manage Long/short jump
        if(rigidbody.velocity.y < 0)
        {
            rigidbody.velocity += Vector3.up * gravity * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigidbody.velocity += Vector3.up * gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
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
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rigidbody.AddForce(moveDirection.normalized * speed, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 currentVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

        if(currentVelocity.magnitude > speed)
        {
            Vector3 limitedVelocity = currentVelocity.normalized * speed;
            rigidbody.velocity = new Vector3(   limitedVelocity.x, 
                                                rigidbody.velocity.y,
                                                limitedVelocity.z);
        }
    }
}
