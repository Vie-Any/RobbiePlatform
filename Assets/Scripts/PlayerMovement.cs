using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    private BoxCollider2D boxCollider2D;

    [Header("Movement settings")]
    // movement speed of the player when the player was standing
    public float speed = 8f;

    // movement speed when the player was crouched
    public float crouchSpeedDivisor = 3f;

    [Header("Jump settings")]
    // jump force of the player
    public float jumpForce = 6.3f;

    // jump force of the player when jump button was hold on
    public float jumpHoldForce = 1.9f;

    // duration time of jump button could be held on 
    public float jumpHoldDuration = 0.1f;

    // extra jump force when the player was crouched
    public float crouchJumpBoost = 2.5f;

    // jump force of the player was hang on the wall
    public float hangingJumpForce = 15f;

    private float jumpTime;

    [Header("State")]
    // mark the state of the player is crouch or not
    public bool isCrouch;

    // mark the state of the player is on the ground or not
    public bool isOnGround;

    // mark the state of the player is jump or not
    public bool isJump;

    // mark the state of the player head is touch the ground of top or not
    public bool isHeadBlocked;

    // mark the state of the player is hang on the wall or not
    public bool isHanging;

    [Header("Environment")]
    // the offset between the player's left foot and right foot
    public float footOffset = 0.4f;

    // the distance at the top of the player's head needs to check 
    public float headClearance = 0.5f;

    // the distance between the player and the ground needs to check
    public float groundDistance = 0.2f;

    // the height of the player
    private float playerHeight;

    // the height of the player's eye
    private float eyeHeight = 1.5f;

    // the distance between the player and the wall
    private float grabDistance = 0.4f;

    // the area range of the player top needs to check
    public float reachOffset = 0.7f;

    // gound layer of the scene
    public LayerMask groundLayer;

    // the force of x axis
    public float xVelocity;

    [Header("Button settings")]
    // mark the state of jump button is pressed or not
    public bool jumpPressed;

    // mark the state of jump button is hold on or not
    public bool jumpHeld;

    // mark the state of crouch button is hold on or not
    public bool crouchHeld;

    // mark the state of crouch button is pressed or not
    public bool crouchPressed;

    // Box Collider 2d original size
    private Vector2 colliderStandSize;
    private Vector2 colliderStandOffSet;
    private Vector2 colliderCrouchSize;
    private Vector2 colliderCrouchOffSet;

    // Start is called before the first frame update
    void Start()
    {
        // init the object reference
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        // the height of the player
        playerHeight = boxCollider2D.size.y;

        colliderStandSize = boxCollider2D.size;
        colliderStandOffSet = boxCollider2D.offset;
        colliderCrouchSize = new Vector2(boxCollider2D.size.x, boxCollider2D.size.y / 2f);
        colliderCrouchOffSet = new Vector2(boxCollider2D.offset.x, boxCollider2D.offset.y / 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // check the input button state
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
        crouchPressed = Input.GetButtonDown("Crouch");
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }

    void PhysicsCheck()
    {
        // Vector2 pos = transform.position;
        // Vector2 offset = new Vector2(-footOffset, 0f);
        //
        // RaycastHit2D leftCheck = Physics2D.Raycast(pos + offset, Vector2.down, groundDistance, groundLayer);
        // Debug.DrawRay(pos + offset, Vector2.down, Color.red, 0.2f);
        // the ray of foot for check does the player was on the ground
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);

        // according to the ray under the player's foot is touching the ground or not to set the player is land on the ground or not
        isOnGround = (leftCheck || rightCheck) ? true : false;

        // the ray of head for check head is touch ground of top
        RaycastHit2D headCheck = Raycast(new Vector2(0f, boxCollider2D.size.y), Vector2.up, headClearance, groundLayer);

        isHeadBlocked = headCheck ? true : false;

        // the direction of the ray needs to be checked
        // the direction is same as the player's x-axis value
        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);

        // check 
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance,
            groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance,
            groundLayer);
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down,
            grabDistance,
            groundLayer);

        if (!isOnGround && rigidbody2D.velocity.y < 0 && ledgeCheck && wallCheck && !blockedCheck)
        {
            Vector3 currentPosition = transform.position;

            currentPosition.x += (wallCheck.distance - 0.05f) * direction;
            currentPosition.y -= ledgeCheck.distance;

            transform.position = currentPosition;

            // change the bodytype of rigidbody to static for keep the player hang on the wall
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            // change the hanging state of the player
            isHanging = true;
        }
    }

    // movement on the ground
    void GroundMovement()
    {
        // when the player was hang on the wall then return
        if (isHanging)
        {
            return;
        }
        // when the crouch button was held on and the player was not crouch state and the player was land on the ground 
        if (crouchHeld && !isCrouch && isOnGround)
        {
            // trigger crouch
            Crouch();
        }
        // when the crouch button was not hold on and the player was crouch state and the player's head is not touching any ground on player's top
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
        {
            // trigger stand up
            StandUp();
        }
        // when the player was not land on the ground and the player is crouch state
        else if (!isOnGround && isCrouch)
        {
            // trigger stand up
            StandUp();
        }

        // set value on x axis movement
        xVelocity = Input.GetAxisRaw("Horizontal");

        // when the player was crouch state
        if (isCrouch)
        {
            // slow down movement speed
            xVelocity /= crouchSpeedDivisor;
        }

        // change the position of the player for implement movement
        rigidbody2D.velocity = new Vector2(xVelocity * speed, rigidbody2D.velocity.y);

        // switch the direction did the player face to
        FlipDirection();
    }

    void MidAirMovement()
    {
        if (isHanging)
        {
            if (jumpPressed)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                rigidbody2D.AddForce(new Vector2(0f,hangingJumpForce),ForceMode2D.Impulse);
                isHanging = false;
            }

            if (crouchPressed)
            {
                rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }
        // if jump button is pressed and released right and the player is land on the ground and the player is not jump state 
        if (jumpPressed && isOnGround && !isJump && !isHeadBlocked)
        {
            // when the player was crouch state and the player is land on the ground
            if (isCrouch)
            {
                // trigger stand up
                StandUp();
                // 
                rigidbody2D.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }

            // change land on the ground state of the player to false
            isOnGround = false;
            // change the jump state of the player to true
            isJump = true;

            // calculate the time of the jump button could be held on
            // the value is the time when the jump button was pressed + duration time of the jump button could be held on 
            jumpTime = Time.time + jumpHoldDuration;
            // add a y-axis force to the player to implement jump effect
            rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        // when the jump button was pressed
        else if (isJump)
        {
            // and the jump button was hold on
            if (jumpHeld)
            {
                // add a y-axis force plus an extra jump force to the player to implement when the jump button was held then the player could jump higher 
                rigidbody2D.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }

            if (jumpTime < Time.time)
            {
                isJump = false;
            }
        }
    }

    // flip the direction of the player face to
    void FlipDirection()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (xVelocity > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    // change the player's state to crouch
    void Crouch()
    {
        // change the crouch state mark value of the player
        isCrouch = true;
        // change the box collider size and offset of the player
        boxCollider2D.size = colliderCrouchSize;
        boxCollider2D.offset = colliderCrouchOffSet;
    }

    // revert the player's state to stand up
    void StandUp()
    {
        // change the crouch state mark value of the player
        isCrouch = false;
        // revert the box collider size and offset of the player
        boxCollider2D.size = colliderStandSize;
        boxCollider2D.offset = colliderStandOffSet;
    }

    /// <summary>
    /// draw a ray for layer touching check
    /// </summary>
    /// <param name="offset">check area range</param>
    /// <param name="rayDirection">direction of the ray</param>
    /// <param name="length">length of the ray</param>
    /// <param name="layer">layer needs to check</param>
    /// <returns></returns>
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        // mark current position of the player
        Vector2 pos = transform.position;

        // check the layer is touching within the area range or not
        // the area needs to check is equals to player's current position + area range
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);

        Color color = hit ? Color.red : Color.green;

        // base on the parameter draw a ray line in GUI for display
        Debug.DrawRay(pos + offset, rayDirection * length, color);

        return hit;
    }
}