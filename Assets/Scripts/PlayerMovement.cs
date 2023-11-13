using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;

    private float dirX = 0f;
    [SerializeField] private float MoveSpeed = 7f;
    [SerializeField] private float JumpForce = 14f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float jumpMultiplier = 1f;
    [SerializeField] private float comboTimer = 3f;
    [SerializeField] private float comboTime = 1f;
    [SerializeField] private Text comboTimerText;
    [SerializeField] private string horizontalInputAxis = "Horizontal";
    [SerializeField] private string jumpInputButton = "Jump";
    public ParticleSystem dust;
    public ParticleSystem jumpParticle;
    public ParticleSystem timerEnd;
    private enum MovementState { Idle, Running, Jumping, Falling }

    private AudioSource audioSource;
    public AudioClip playerRun;
    public AudioClip playerJump;
    public AudioClip jumpComboPoof;

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
        if (current.name.Equals("Two Player Mode"))
        {
            // Assign input keys for Player 1 and Player 2
            if (gameObject.CompareTag("Player1"))
            {
                horizontalInputAxis = "Horizontal_P1";
                jumpInputButton = "Jump_P1";
            }
            else if (gameObject.CompareTag("Player2"))
            {
                horizontalInputAxis = "Horizontal_P2";
                jumpInputButton = "Jump_P2";
            }
        }
    }

    private void Update()
    {
        if (IsGrounded())
        {
            comboTime += ((1 + jumpMultiplier) * Time.deltaTime);
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        dirX = Input.GetAxisRaw(horizontalInputAxis);
        rb.velocity = new Vector2(dirX * MoveSpeed, rb.velocity.y);

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            comboTimer = 3;
            jumpBufferCounter = 0;
            rb.velocity = new Vector2(rb.velocity.x, (JumpForce * jumpMultiplier));
            jumpMultiplier += .1f;
            audioSource.PlayOneShot(playerJump, .35f);
            CreateJumpParticle();
            CreateDust();
        }

        if (Input.GetButtonUp(jumpInputButton) && rb.velocity.y > 0f)
        {
            coyoteTimeCounter = 0f;
        }

        if (Input.GetButtonDown(jumpInputButton))
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
            audioSource.PlayOneShot(jumpComboPoof, 1f);
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
            if (!audioSource.isPlaying && IsGrounded())
            {
                audioSource.PlayOneShot(playerRun, .25f);
            }
            state = MovementState.Running;
            sprite.flipX = false;
            CreateDust();
        }
        else if (dirX < 0f)
        {
            if (!audioSource.isPlaying && IsGrounded())
            {
                audioSource.PlayOneShot(playerRun, .25f);
            }
            state = MovementState.Running;
            sprite.flipX = true;
            CreateDust();
        }
        else
        {
            state = MovementState.Idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.Jumping;

            if (jumpParticle.isPlaying == false)
            {
                UpdateParticleColor(jumpParticle);
                CreateJumpParticle();
            }
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.Falling;
            if (jumpParticle.isPlaying == true)
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

    private void CreateDust()
    {
        dust.Play();
    }

    private void CreateJumpParticle()
    {
        jumpParticle.Play();
    }

    private void UpdateParticleColor(ParticleSystem part)
    {
        var mainPS = part.main;
        if (comboTime <= 1f)
        {
            mainPS.startColor = Color.yellow;
        }
        else if (comboTime <= 2f)
        {
            mainPS.startColor = Color.white;
        }
        else if (comboTime <= 3f)
        {
            mainPS.startColor = Color.gray;
        }
    }
}
