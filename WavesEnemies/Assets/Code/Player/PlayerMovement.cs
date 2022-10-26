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
            Animate(lastMoveDirection);
            rigidBody.velocity = Vector2.zero;
        }
        //Debug.Log(lastMoveDirection);
    }

    private void LateUpdate()
    {
        this.AnimationStateSwitch();
        base.animator.SetInteger("state", (int)state);
        //Debug.Log(state.ToString());
        //Animate();
    }

    private void ProcessInput()
    {
        move_X = Input.GetAxisRaw("Horizontal");
        move_Y = Input.GetAxisRaw("Vertical");

        //if ((move_X == 0 && move_Y == 0) && moveDirection.x != 0 || moveDirection.y != 0)
        //{
        //lastMoveDirection = moveDirection;       
        //}
        if (move_X == 0 && move_Y == 0)
        {
                 
        }

        moveDirection = new Vector2(move_X, move_Y).normalized;
        lastMoveDirection = moveDirection;
        Move();       
    }

    private void Move()
    {
        rigidBody.velocity = new Vector2(moveDirection.normalized.x * movingSpeed, moveDirection.normalized.y * movingSpeed);
    }

    protected void AnimationStateSwitch()
    {
        if (Input.anyKey)
        {
            if (Mathf.Abs(base.rigidBody.velocity.x) > minimumVelocity || Mathf.Abs(base.rigidBody.velocity.y) > minimumVelocity)
            {
                state = AnimationState.moving;
                Animate(moveDirection);
            }
        }
        else
        {
            state = AnimationState.idle;
        }
    }

    private void Animate(Vector2 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        //animator.SetFloat("AnimMoveMagnitude", moveDirection.magnitude);

        //animator.SetFloat("AnimLastMoveX", lastMoveDirection.x);
        //animator.SetFloat("AnimLastMoveY", lastMoveDirection.y);
    }
}
