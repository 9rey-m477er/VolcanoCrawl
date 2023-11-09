using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float comboTimer = 3f;
    [SerializeField] private float comboTime = 1f;
    [SerializeField] private Text comboTimerText;
    public ParticleSystem dust;
    public ParticleSystem jumpParticle;
    public ParticleSystem timerEnd;
    private enum MovementState {Idle,Running, Jumping, Falling}

    private AudioSource audioSource;
    public AudioClip playerRun;
    public AudioClip playerJump;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        audioSource = GetComponent<AudioSource>();

        Scene current = SceneManager.GetActiveScene();
        if(current.name.Equals("Two Player Mode"))
        {

        }
    }
    // Start is called before the first frame update
    //private void Start()
    //{

    //    rb=GetComponent<Rigidbody2D>();
    //    anim=GetComponent<Animator>();
    //    sprite=GetComponent<SpriteRenderer>();
    //    coll=GetComponent<BoxCollider2D>();

    //    audioSource=GetComponent<AudioSource>();
    //}

    // Update is called once per frame
    private void Update()
    {
        if (IsGrounded())
        {
            comboTime += ((1+jumpMultiplier) * Time.deltaTime);
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * MoveSpeed, rb.velocity.y);
        //changed IsGrounded check to CoyoteTime check to add coyote time - Reece
        //added Jump Buffering by changing Input.GetButtonDown("Jump") to jumpBufferCounter > 0f
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            comboTimer = 3;
            jumpBufferCounter = 0; // buffer counter change
            rb.velocity = new Vector2(rb.velocity.x, (JumpForce * jumpMultiplier));
            jumpMultiplier += .1f;
            audioSource.PlayOneShot(playerJump, .35f);
            CreateJumpParticle();
            CreateDust();
        }
        //added this as well for coyote timer implementation - Reece
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            coyoteTimeCounter = 0f;
        }
        //added this for jump buffer implementation - Reece
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (comboTime >= comboTimer)
        {
            timerEnd.Play();
            comboTime = 0f;
            jumpMultiplier = 1;
        }

        UpdateAnimationState();
        UpdateParticleColor(dust);
        UpdateParticleColor(jumpParticle);

        comboTimerText.text = "Jump Multiplier: " + jumpMultiplier + "x";
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
            CreateDust();
        }
        else if(dirX < 0f)
        {
            if (!audioSource.isPlaying && IsGrounded())
            {
                audioSource.PlayOneShot(playerRun, .25f);
            }
            state =MovementState.Running;
            sprite.flipX = true;
            CreateDust();
        }
        else
        {
            state=MovementState.Idle;
        }
        if (rb.velocity.y > .1f)
        {
            state=MovementState.Jumping;

            if(jumpParticle.isPlaying == false)
            {
                UpdateParticleColor(jumpParticle);
                CreateJumpParticle();
            }  
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
            if(jumpParticle.isPlaying == true)
            {
                jumpParticle.Stop();
            }
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    void CreateDust()
    {
        dust.Play();
    }
    void CreateJumpParticle()
    {
        jumpParticle.Play();
    }
    void UpdateParticleColor(ParticleSystem part)
    {
        var mainPS = part.main;
        if(comboTime <= 1f)
        {
            mainPS.startColor = Color.yellow;
        }
        else if(comboTime <= 2f)
        {
            mainPS.startColor = Color.white;
        }
        else if (comboTime <= 3f)
        {
            mainPS.startColor = Color.gray;
        }
    }
}
