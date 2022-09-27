using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Main class that contains all the player movement behaviour.
    /// </summary>
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class PlayerMovement3D : MonoBehaviour
    {
        [Header("Horizontal movement")]
        [Min(0)] public float moveSpeed = 20f;
        [Min(0)] public float maxSpeed = 25f;
        [Tooltip("Time needed to make a turn on its position")]
        [Min(0)] public float turnTime = 0.1f;
        private float turnVelocity;
        private float originalSpeed;
        private Vector3 inputDirection;
        private Vector3 moveDirection;
        private bool canMove = true;

        [Header("Camera movement")]
        new public Transform camera;

        [Header("Slope Movement")]
        [Tooltip("Maximum angle to consider a slope (greater values means walls)")]
        [Min(0)] public float maxSlopeAngle = 60f;
        [Tooltip("Minimum angle to consider a slope (lower values means floor)")]
        [Min(0)] public float minSlopeAngle = 15f;
        [Min(0)] public float onSlopeSpeed = 5f;
        private RaycastHit slopeHit;

        [Header("Jumping")]
        [Min(0)] public float jumpHeight = 12f;
        [Tooltip("Value determining how fast the player will fall")]
        public float fallMultiplier = 2.5f;
        [Tooltip("Value determining how far will the long jump go")]
        public float lowJumpMultiplier = 4f;
        private float gravity = -9.81f;
        private Vector3 velocity;

        [Header("Diving")]
        [Min(0)] public float diveForce = 5f;
        [Tooltip("Time suspended in the air before diving")]
        [Min(0)] public float airTimeWait = 0.2f;
        [Tooltip("Minimum time so the player can dive again")]
        [Min(0)] public float canDiveStart = 0.2f;
        private bool canDive = false;

        [Header("Wall jumping")]
        [Min(0)] public float wallDistance = 0.7f;
        [Min(0)] public float minJumpHeight = 0.75f;
        [Min(0)] public float wallJumpForce = 7f;
        [Min(0)] public float wallJumpSideForce = 12f;
        [Tooltip("Minimum time so the player can wall jump again")]
        [Min(0)] public float exitWallTime = 0.7f;
        [Tooltip("Check what layer is considered wall")]
        public LayerMask wallMask;
        private RaycastHit wallHit;
        private bool wallFound = false;

        [Header("Ground controller")]
        [Tooltip("Distance to check for the ground under the player")]
        [Min(0)] public float groundDistance = 0.4f;
        [Tooltip("Transform component to position the groundCheck attribute")]
        public Transform groundCheck;
        [Tooltip("Check what layer is considered ground")]
        public LayerMask groundMask;
        private bool isGrounded;

        [Header("Crouching")]
        [Min(0)] public float crouchSpeed = 3.5f;
        [Tooltip("Height the player will have when crouching")]
        [Min(0)] public float crouchHeight = 0.5f;
        private float originalHeight;

        [Header("Dashing")]
        [Min(0)] public float dashForce = 3f;
        [Tooltip("Maximum time to consider a double tap")]
        [Min(0)] public float doubleTapTime = 0.4f;
        [Tooltip("Minimum time so the player can double tap again")]
        [Min(0)] public float doubleTapCoolDown = 0.4f;
        private float doubleTapResetTime = 0f;
        private bool canDash = true;
        private float doubleTapDelta;
        private bool doubleTap = false;
        private bool enableGlitch = false;

        [Header("Keyboard inputs")]
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode diveInput = KeyCode.LeftControl;
        public KeyCode crouchInput = KeyCode.LeftShift;

        [Header("Secret Code")]
        [Tooltip("List of KeyCode names to specify a secret code (enabling glitches)")]
        public string[] secretCode;
        public float inputTimeReset = 1.0f;
        private string secretCodeString;
        private string currentCode = "";

        // GameObject components
        new private Rigidbody rigidbody;
        private AudioAndVideoManager anim;
        new private ParticleSystem particleSystem;

        // ----------------------------------------------------------------------------------------------- Unity Methods
        void Start()
        {
            // Initialize private components
            rigidbody = GetComponent<Rigidbody>();
            anim = GetComponent<AudioAndVideoManager>();
            particleSystem = transform.GetChild(transform.childCount - 1).gameObject.GetComponent<ParticleSystem>();
            particleSystem.Pause();
            
            // Establish original values
            originalHeight = transform.localScale.y;
            originalSpeed = moveSpeed;
            
            ConfigSecretInput();
        }

        void Update()
        {
            SendAnimationVals();
            CheckGrounded();
            CheckWallJump();
            CheckDash();
            CheckResetDash();
            CheckCrouch();
            CheckJump();
            CheckDive();
            CheckSecretInput();
            SpeedControl();
        }

        void FixedUpdate()
        {
            CheckInputs();
            CheckMove();
            SpeedControl();
        }

        // ----------------------------------------------------------------------------------------------- Private Methods
        /// <summary>
        /// This method sends animation states to the Audio and Video Manager component.
        /// </summary>
        private void SendAnimationVals()
        {
            anim.IsOnGround(isGrounded);
            anim.SetIfMovement(rigidbody.velocity.magnitude);
            anim.IsOnWall(wallFound);
        }

        /// <summary>
        /// This method checks if the player is on ground.
        /// </summary>
        private void CheckGrounded()
        {
            //Check if object is grounded by creating an invisible sphere
            //and checking if anything contained in groundMask is in contact with it
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            // Creates a force down to stay in ground
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;

            if (isGrounded)
            {
                canDive = false;
                // Deactivate smoke
                particleSystem.gameObject.SetActive(false);
                particleSystem.Pause();
            }
        }

        /// <summary>
        /// This method checks if the player can wall jump.
        /// </summary>
        private void CheckWallJump()
        {
            // Looks for a wall in front of the player with a raycast, returns a RaycastHit if true
            wallFound = Physics.Raycast(transform.position, transform.forward, out wallHit, wallDistance, wallMask);

            if (wallFound && AboveGround())
                if (Input.GetKeyDown(jumpInput))
                    WallJump();
        }

        /// <summary>
        /// This method checks if the player can dash by double tapping either up key or W key.
        /// </summary>
        private void CheckDash()
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && doubleTap)
            {
                if (Time.time - doubleTapDelta < doubleTapTime)
                {
                    Dash();
                    doubleTapResetTime = doubleTapCoolDown; // Reset Dash
                    doubleTapDelta = 0f;
                }
                doubleTap = false;
            }

            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !doubleTap)
            {
                doubleTap = true;
                doubleTapDelta = Time.time;
            }
        }

        /// <summary>
        /// This method resets the dash used previously.
        /// </summary>
        private void CheckResetDash()
        {
            if(enableGlitch)
                return;
            
            // Reduce the doubleTapResetTime counter until it hits 0, then reset dash
            if(doubleTapResetTime > 0)
            {
                canDash = false;
                doubleTapResetTime -= Time.deltaTime;
                if(doubleTapResetTime == 0)
                    doubleTapResetTime = doubleTapCoolDown;
            }
            else if(doubleTapResetTime <= 0)
                canDash = true;
        }

        /// <summary>
        /// This method checks if the player can crouch when holding crouch key.
        /// </summary>
        private void CheckCrouch()
        {
            if (Input.GetKeyDown(crouchInput))
            {
                transform.localScale = new Vector3(transform.localScale.x,
                                                    crouchHeight,
                                                    transform.localScale.z);
                rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                moveSpeed = crouchSpeed;
            }

            if (Input.GetKeyUp(crouchInput))
            {
                transform.localScale = new Vector3(transform.localScale.x,
                                                    originalHeight,
                                                    transform.localScale.z);
                moveSpeed = originalSpeed;
            }
        }

        /// <summary>
        /// This method checks if the player can jump and manages long/short jump.
        /// </summary>
        private void CheckJump()
        {
            if (Input.GetKeyDown(jumpInput) && isGrounded)
            {
                StartCoroutine(EnableDive());
                anim.jumpSound();
                rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }

            // Checks if player is falling
            if (rigidbody.velocity.y < 0)
            {
                rigidbody.velocity += Vector3.up * gravity * (fallMultiplier - 1) * Time.deltaTime;
                // Activate smoke
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
            }
            // check if the player is still going up while jumping and holding the jump button
            else if (rigidbody.velocity.y > 0 && !Input.GetKey(jumpInput))
                rigidbody.velocity += Vector3.up * gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        /// <summary>
        /// This method checks if the player can dive.
        /// </summary>
        private void CheckDive()
        {
            if (canDive && Input.GetKeyDown(diveInput) && !wallFound)
            {
                anim.DiveSound();
                StartCoroutine(Dive());
            }
        }

        /// <summary>
        /// This method checks for the secret input command.
        /// </summary>
        private void CheckSecretInput()
        {
            if(enableGlitch)
                return;
            
            // Checks every key pressed and evaluates if it corresponds to the secret code
            foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if(Input.GetKeyDown(kcode))
                {
                    currentCode += kcode;
                    if(currentCode.Contains(secretCodeString))
                    {
                        // Secret code succesfully typed
                        enableGlitch = true;
                        anim.UnlockSound();
                    }
                    // Limit currentCode storage data (3 times the length of the secret code)
                    else
                        if(currentCode.Length > secretCodeString.Length * 3)
                            currentCode = "";
                }
            }
        }

        /// <summary>
        /// This method limits the global velocity of the player.
        /// </summary>
        private void SpeedControl()
        {
            rigidbody.velocity = new Vector3(
                Mathf.Clamp(rigidbody.velocity.x, -maxSpeed, maxSpeed),
                Mathf.Clamp(rigidbody.velocity.y, -maxSpeed, maxSpeed),
                Mathf.Clamp(rigidbody.velocity.z, -maxSpeed, maxSpeed)
                );
        }

        /// <summary>
        /// This method checks for the player inputs involving movement.
        /// </summary>
        private void CheckInputs()
        {
            //Gather Keyboard Input and create resulting vector
            //Normalized to avoid faster movement in diagonals
            float horizontalInput = 0f;
            float verticalInput = 0f;

            if (!wallFound || isGrounded)
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
                verticalInput = Input.GetAxisRaw("Vertical");
            }

            inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        }

        /// <summary>
        /// This method manages the movement of the player.
        /// </summary>
        private void CheckMove()
        {
            if (!canMove)
                return;
            
            // Moves if any input key is pressed
            if (inputDirection.magnitude >= 0.1f)
            {
                // Utilize Atan2 function to find angle player should look at based on direction vector and camera angle
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
                // Utilize SmoothDampAngle function to change the angle based on established variables for a smoother look
                float resultAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, turnTime);
                
                transform.rotation = Quaternion.Euler(0f, resultAngle, 0f);
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                if (OnSlope())
                {
                    // If on slope, an extra force is applied to the horizontal movement depending on the slope angle
                    rigidbody.AddForce(Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized * moveSpeed * onSlopeSpeed, ForceMode.Force);
                    // Adds an extra force down to keep the player on ground
                    if (rigidbody.velocity.y > 0)
                        rigidbody.AddForce(Vector3.down * 3, ForceMode.Force);
                }
                else
                    rigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
            }
        }

        /// <summary>
        /// This method adds forces to the player so that they can wall jump.
        /// </summary>
        private void WallJump()
        {
            if (!canMove)
                return;

            Vector3 wallNormal = wallHit.normal;
            Vector3 jumpForce = transform.up * wallJumpForce + wallNormal * wallJumpSideForce;

            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
            rigidbody.AddForce(jumpForce, ForceMode.Impulse);

            transform.Rotate(new Vector3(0, 180, 0));
            StartCoroutine(ResetWallJump());
        }

        /// <summary>
        /// This method adds forces to the player so that they can dash.
        /// </summary>
        private void Dash()
        {
            if(canDash)
                rigidbody.AddForce(rigidbody.velocity * dashForce, ForceMode.Impulse);
        }

        /// <summary>
        /// This method creates a string value using the list of keyCode inputs from the secretCode list.
        /// </summary>
        private void ConfigSecretInput()
        {
            foreach (string input in secretCode)
                secretCodeString += input;
        }

        /// <summary>
        /// This method checks if the player is on a slope.
        /// </summary>
        /// <returns>Returns a bool value wether the player is on a slope or not. </returns>
        private bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down,
            out slopeHit, groundCheck.position.y + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle > minSlopeAngle && angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        /// <summary>
        /// This method checks if the player is not on ground.
        /// </summary>
        /// <returns>Returns a bool value wether the player is above certain altitude from the ground. </returns>
        private bool AboveGround()
        {
            return !Physics.Raycast(transform.position, Vector3.down, 2, groundMask);
        }

        // ----------------------------------------------------------------------------------------------- Private Coroutines

        /// <summary>
        /// This method waits for the exitWallTime time variable to enable movement back.
        /// </summary>
        private IEnumerator ResetWallJump()
        {
            canMove = false;
            yield return new WaitForSeconds(exitWallTime);
            canMove = true;
        }

        /// <summary>
        /// This method waits for the canDiceStart time variable to enable diving again.
        /// </summary>
        private IEnumerator EnableDive()
        {
            yield return new WaitForSeconds(canDiveStart);
            canDive = true;
        }

        /// <summary>
        /// This method creates the movement of the dive.
        /// </summary>
        private IEnumerator Dive()
        {
            // Stops player movement in the air
            canMove = false;
            canDive = false;
            rigidbody.useGravity = false;
            rigidbody.velocity = Vector3.zero;
            yield return new WaitForSeconds(airTimeWait);

            // Adds forces to the player so they can dive
            rigidbody.AddForce(this.transform.forward * diveForce + Vector3.up * (jumpHeight / 2), ForceMode.Impulse);// new Vector3(moveDirection.x  * diveForce, (jumpHeight/3)  * diveForce , moveDirection.z * diveForce), ForceMode.Impulse);
            rigidbody.useGravity = true;
            yield return new WaitForSeconds(airTimeWait / 2);

            // Enables movement back
            canMove = true;
        }
    }
}