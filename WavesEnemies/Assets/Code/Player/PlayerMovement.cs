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
    //internal bool moveKeyIsPressed;
    float move_X;
    float move_Y;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    private void Update()
    {     
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            ProcessInput();
        }
        else
        {
            move_X = 0;
            move_Y = 0;
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        this.AnimationStateSwitch();
        base.animator.SetInteger("state", (int)state);
        //Animate();
    }

    private void ProcessInput()
    {
        move_X = Input.GetAxisRaw("Horizontal");
        move_Y = Input.GetAxisRaw("Vertical");

        if ((move_X == 0 && move_Y == 0) && moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
        }

        moveDirection = new Vector2(move_X, move_Y).normalized;
        Move();       
    }

    private void Move()
    {
        Debug.Log(moveDirection);

        rigidBody.velocity = new Vector2(moveDirection.normalized.x * movingSpeed, moveDirection.normalized.y * movingSpeed);
    }

    protected void AnimationStateSwitch()
    {
        if (Input.anyKey)
        {
            if (Mathf.Abs(base.rigidBody.velocity.x) > minimumVelocity || Mathf.Abs(base.rigidBody.velocity.y) > minimumVelocity)
            {
                state = AnimationState.moving;
                Animate();
            }
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
