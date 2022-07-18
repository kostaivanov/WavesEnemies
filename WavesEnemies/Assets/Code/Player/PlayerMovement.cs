using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerMovement : PlayerComponents
{
    #region Constants
    private const float minimumVelocity_X = 1f;
    private const float minimumFallingVelocity_Y = -2f;
    private const float groundCheckRadius = 0.1f;
    #endregion

    #region Serialized Fields
    [SerializeField] private AudioSource playerAudioSource;
    //[SerializeField] private AudioClip shootSound;

    [SerializeField] internal float movingSpeed;
    //[SerializeField] Transform playerGroundCheck;
    #endregion

    private float direction;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        this.AnimationStateSwitch();
        base.animator.SetInteger("state", (int)state);
    }

    private void ProcessInput()
    {
        float move_X = Input.GetAxisRaw("Horizontal");
        float move_Y = Input.GetAxisRaw("Vertical");

        if ((move_X == 0 && move_Y == 0) && moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(move_X, move_Y).normalized;
    }

    private void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * movingSpeed, moveDirection.y * movingSpeed);
    }

    protected void AnimationStateSwitch()
    {

    }
}
