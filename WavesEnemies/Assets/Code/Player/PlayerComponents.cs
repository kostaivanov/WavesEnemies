using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class PlayerComponents : MonoBehaviour
{
    #region Components
    protected Rigidbody2D rigidBody;
    protected Collider2D collider2D;
    internal Animator animator;
    //protected PlayerMovement playerMovement;
    //protected PlayerHealth playerHealth;
    // internal LayerMask groundLayer;
    protected SpriteRenderer playerSprite;
    #endregion

    internal AnimationState state = AnimationState.idle;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        //groundLayer = LayerMask.GetMask("GroundLayer");
        playerSprite = GetComponent<SpriteRenderer>();
    }
}
