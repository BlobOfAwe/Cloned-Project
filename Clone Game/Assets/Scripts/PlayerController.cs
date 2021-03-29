using System;
using UnityEngine;

/// <summary>
/// Controls the player
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private GameManager gameManager;
    private Animator animator;
    private PlayerEffectController effectController;
    public bool isControllable; // Is the player controllable
    [SerializeField] float gravity; // The effect of gravity on the player (In meters per second squared)
    private float xInput; // Input from the user along the x axis
    [SerializeField] float yInput; // Input from the user along the y axis
    [SerializeField] float yVelocity; // Player's velocity along the y axis
    private float xVelocity; // Player's velocity along the x axis

    [Header("Grounding and Jumping")]
    [SerializeField] LayerMask whatIsGround; // What layers are considered ground?
    [SerializeField] LayerMask whatIsCeiling; // What layers are considered to be a ceiling
    [SerializeField] GameObject groundCheck; // If this object touches the ground, the player is on the ground
    [SerializeField] float groundDetectRadius; // Distance that groundCheck checks to see if it is on the ground
    public bool isGrounded; // Is the player on the ground
    [SerializeField] bool isJumping; // Is the player jumping?
    [SerializeField] GameObject ceilingCheck; // If the object touches the ceiling, the player has hit a ceiling
    [SerializeField] float ceilingDetectRadius; // Distance that ceilingCheck checks around to see if it has hit a ceiling
    [SerializeField] bool hitCeiling; // HAs the player hit a ceiling?
    [SerializeField] float jumpStartHeight; // What height did the player start jumping at?

    [Header("Stats")]
    [SerializeField] float runSpeed; // in Meters per Second
    [SerializeField] float jumpSpeed; // in Meters per Second
    [SerializeField] float maxJumpHeight; // in Meters

    // As soon as the gameObject loads...
    private void Awake()
    {
        // Identify player's components
        playerRB = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        effectController = GetComponent<PlayerEffectController>();
        
        // Set velocity to 0
        xVelocity = 0;
        yVelocity = 0;
        gravity = playerRB.gravityScale; // Set player's gravity
    }


    private void Update()
    {
        animator.SetBool("IsJumping", !isGrounded); // If the player is not grounded, they are jumping. (sets the bool in the animator)
        animator.SetBool("IsControllable", isControllable); // Sets isControllable in the animator as well

        xInput = Input.GetAxis("Horizontal"); // Binds xInput to the Horizontal Input axis
        yInput = Input.GetAxisRaw("Vertical"); // Binds yInput to the Vertical input axis

        JumpCheck();
    }

    // All physics calculations happen here
    void FixedUpdate()
    {
        if (GameManager.gameState != 2) { playerRB.gravityScale = 0; } // If the level is not in progress, there should be zero gravity
        else { playerRB.gravityScale = gravity; } // Otherwise set the gravity to the assigned variable

        yVelocity = Convert.ToSingle(Math.Round(playerRB.velocity.y, 2)); // Display the player's y velocity to the nearest hundreth
        xVelocity = Convert.ToSingle(Math.Round(playerRB.velocity.x, 2)); // Display the player's x velocity to the nearest hundredth

        animator.SetFloat("xSpeed", Math.Abs(xVelocity)); // Set the animator variable xSpeed to the absolute (positive) value of xVelocity
        
        // Run all the motion Functions
        GroundedCheck();
        CeilingCheck();
        Run();
        if (isJumping) { Jump(); }
        Flip();
    }

    // Checks to see if the player is Grounded
    void GroundedCheck()
    {
        // Sets a CircleCast at groundCheck's position, with groundDetectRadius, checking for layers included in whatIsGround
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundDetectRadius, whatIsGround);

        // If the cast detected at least 1 collider labelled ground, continue.
        if (colliders.Length != 0)
        {
            // Check to see if any colliders detected are the player
            for (int i = 0; i < colliders.Length; i++)
            {
                // Declare the player isGrounded if the CircleCast found at least one ground layer that is not this gameobject
                if (colliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                }

                // If the only layers found were the player
                else { isGrounded = false; }
            }
        }

        // If the cast detected no colliders
        else { isGrounded = false;}

    }

    // Checks to see if the player is blocked by a ceiling
    void CeilingCheck()
    {
        // Sets a CircleCast at groundCheck's position, with groundDetectRadius, checking for Ceiling layers
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ceilingCheck.transform.position, ceilingDetectRadius, whatIsCeiling);

        // If the cast detected at least 1 collider, continue.
        if (colliders.Length != 0)
        {
            // Check to see if any colliders detected are ground
            for (int i = 0; i < colliders.Length; i++)
            {
                // Declare the player hitCeiling if the CircleCast found Ceiling
                if (colliders[i].gameObject != gameObject)
                {
                    hitCeiling = true;
                }

                // If none of the colliders were Ceiling
                else { hitCeiling = false; }
            }
        }

        // If the cast detected no colliders
        else { hitCeiling = false; }

    }

    // Is the player jumping?
    void JumpCheck()
    {
        // If the player presses the Jump key, and they are able to jump...
        if (Input.GetButton("Jump") && isGrounded && isControllable)
        {
            StartCoroutine(effectController.PlayJump()); // Play the Jump effect
            jumpStartHeight = transform.position.y; // Identify the jump start height
            isJumping = true; // The player is Jumping
        }

        if (Input.GetButtonUp("Jump")) { isJumping = false; } // If the player releases the jump key, stop jumping


    }

    // Checks to see if the player is jumping, and jumps. Should run every FixedUpdate.
    void Jump()
    {
        // If the Jump button is being held, and the player has not reached the maximum jump height
        // and the player has not hit a ceiling, continue the jump
        if
        (
            // Conditions
            Input.GetButton("Jump")
            && transform.position.y < jumpStartHeight + maxJumpHeight
            && !hitCeiling
            && isControllable
        )

        {
            // Function
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpSpeed); // Set the player's velocity to make them jump
        }

        else { isJumping = false; } // If one of the conditions becomes false, stop jumping

    }

    // Sets the player's xVelocity based on xInput, should run every FixedUpdate
    void Run()
    {
        // If the player is controllable...
        if (isControllable)
        {
            playerRB.velocity = new Vector2(xInput * runSpeed, playerRB.velocity.y); // Set the player's velocity
        }
    }

    // flip the player to face whichever direction they are moving
    void Flip()
    {
        if (playerRB.velocity.x > 0) { transform.rotation = Quaternion.Euler(0, 0, 0); } // If the player's velocity is positive, face right

        else if (playerRB.velocity.x < 0) { transform.rotation = Quaternion.Euler(0, 180, 0); } // If the player's velocity is negative, face left

    }

   
    // Draws the ground and ceiling checks in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundDetectRadius);
        Gizmos.DrawWireSphere(ceilingCheck.transform.position, ceilingDetectRadius);
    }
}
