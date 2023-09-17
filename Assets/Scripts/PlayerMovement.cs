using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    
    private float dirX=0f;
    [SerializeField]private float MoveSpeed = 7f;
    [SerializeField]private float JumpForce=14f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float jumpMultiplier = 1f;
    private enum MovementState {Idle,Running, Jumping, Falling}

    private AudioSource audioSource;
    public AudioClip playerRun;
    public AudioClip playerJump;

    // Start is called before the first frame update
    private void Start()
    {
     
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponent<Animator>();
        sprite=GetComponent<SpriteRenderer>();
        coll=GetComponent<BoxCollider2D>();

        audioSource=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * MoveSpeed, rb.velocity.y);
        if (Input.GetButtonDown("Jump")&&IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, (JumpForce * jumpMultiplier));
            jumpMultiplier += .1f;
            audioSource.PlayOneShot(playerJump, .35f);
        }
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        MovementState state; 
        if (dirX > 0f)
        {
            //makes sure audio doesn't loop and only plays when moving while grounded
            if(!audioSource.isPlaying && IsGrounded())
            {
                audioSource.PlayOneShot(playerRun, .25f);
            }
            state = MovementState.Running;
            sprite.flipX = false;
        }
        else if(dirX < 0f)
        {
            if (!audioSource.isPlaying && IsGrounded())
            {
                audioSource.PlayOneShot(playerRun, .25f);
            }
            state =MovementState.Running;
            sprite.flipX = true;
        }
        else
        {
            state=MovementState.Idle;
        }
        if (rb.velocity.y > .1f)
        {
            state=MovementState.Jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
