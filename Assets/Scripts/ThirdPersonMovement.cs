using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    //Variables needed for character movement and camera functionality
    public CharacterController controller;
    public Transform camera;
    public float speed = 6f;
    public float jumpHeight = 3f;
    
    //Variables needed for camera turning
    float turnTime = 0.1f;
    float turnVelocity;

    //Variables needed for gravity functionality
    Vector3 velocity;
    public float gravity =  -9.81f;

    //Variables needed to check ground below player
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isGrounded);
        //Check if object is grounded by creating an invisible sphere
        //and checking if anything contained in groundMask is in contact with it
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
            velocity.y = -2f;


        //Gather Keyboard Input and create resulting vector
        //Normalized to avoid faster movement in diagonals
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if(direction.magnitude >= 0.1f)
        {
            //Utilize Atan2 function to find angle player should look at based on direction vector and camera angle
            //Utilize SmoothDampAngle function to change the angle based on established variables for a smoother look
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float resultAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, resultAngle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        //Gravity Control
        if(Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
