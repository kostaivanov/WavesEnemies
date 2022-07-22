using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerMovement : PlayerComponents
{
    #region Constants
    private const float minimumVelocity = 1f;
    private const float minimumFallingVelocity_Y = -2f;
    private const float groundCheckRadius = 0.1f;
    #endregion

    #region Serialized Fields
    [SerializeField] private AudioSource playerAudioSource;
    //[SerializeField] private AudioClip shootSound;

    [SerializeField] internal float movingSpeed;
    #endregion

    private Vector2 moveDirection;
    private Vector3 aim;
    private Vector2 lastMoveDirection;
    internal bool moveKeyIsPressed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveKeyIsPressed = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.anyKey == true)
        {
            moveKeyIsPressed = true;
        }
        if (moveKeyIsPressed == true)
        {
            ProcessInput();
        }
        //aim = new Vector3(GetAX)
    }

    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        this.AnimationStateSwitch();
        base.animator.SetInteger("state", (int)state);
        Animate();
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
        Move();
    }

    private void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.x * movingSpeed, moveDirection.y * movingSpeed);
    }

    protected void AnimationStateSwitch()
    {
        if (Input.anyKey)
        {
            if (Mathf.Abs(base.rigidBody.velocity.x) > minimumVelocity || Mathf.Abs(base.rigidBody.velocity.y) > minimumVelocity)
            {

            }
            state = AnimationState.moving;
        }
        else
        {
            state = AnimationState.idle;
        }
    }

    private void Animate()
    {
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        //animator.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);

        //animator.SetFloat("AnimLastMoveX", lastMoveDirection.x);
        //animator.SetFloat("AnimLastMoveY", lastMoveDirection.y);

    }
}
