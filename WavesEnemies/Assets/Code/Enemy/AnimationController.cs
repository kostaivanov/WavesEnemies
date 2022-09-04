using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private EnemyMovement movement;
    internal AnimationState state = AnimationState.idle;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("movement.placePlatformButton = " + movement.placePlatformButton.putPlatformClicked);
        this.AnimationStateSwitch();
        animator.SetInteger("state", (int)state);
    }

    protected void AnimationStateSwitch()
    {
        if (movement.placePlatformButton == true)
        {
                state = AnimationState.moving;
        }
        else
        {
            state = AnimationState.idle;
        }
    }
}
