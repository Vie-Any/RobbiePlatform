using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

    private PlayerMovement movement;

    private Rigidbody2D rb;

    [SerializeField]
    // hash id of animator parameter
    private int groundID;

    private int hangingID;
    private int crouchID;
    private int speedID;
    private int fallID;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        groundID = Animator.StringToHash("isOnGround");
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        speedID = Animator.StringToHash("speed");
        fallID = Animator.StringToHash("verticalVelocity");
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(speedID, Mathf.Abs(movement.xVelocity));
        animator.SetBool(groundID, movement.isOnGround);
        animator.SetBool(hangingID, movement.isHanging);
        animator.SetBool(crouchID, movement.isCrouch);
        animator.SetFloat(fallID, rb.velocity.y);
    }

    public void StepAudio()
    {
        AudioManage.PlayFootstepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManage.PlayCrouchFootstepAudio();
    }
}