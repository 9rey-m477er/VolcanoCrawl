using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    private bool isStunned = false;
    private float stunDuration = 0.5f;
    private float stunTimer = 0.0f;

    AudioSource source;
    public AudioClip stunSound;

    private Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D component.
    private GameObject player1;
    private SpriteRenderer player1SpriteRenderer;

    [SerializeField] private Material originalPlayer1Mat;
    [SerializeField] private Material flashMaterial;

    private Coroutine currentCoroutine;

    private void Start()
    {
        // Find and store a reference to the player's Rigidbody2D component.
        playerRigidbody = GameObject.FindGameObjectWithTag("Player1").GetComponent<Rigidbody2D>();
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player1SpriteRenderer = player1.GetComponent<SpriteRenderer>();
        source = playerRigidbody.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            if(source.isPlaying != true)
            {
                source.PlayOneShot(stunSound, .7f);
            }
            stunTimer += Time.deltaTime;
            DisableHorizontalMovement();
            if (stunTimer >= stunDuration)
            {
                isStunned = false;
                stunTimer = 0.0f;

                // Allow the player to move horizontally again when the stun duration is over.
                EnableHorizontalMovement();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player1")||collision.gameObject.tag.Equals("Player2") && !isStunned)
        {
            Debug.Log("Stunned");
            StunPlayer();
            Flash();
        }
    }

    void StunPlayer()
    {
        isStunned = true;
        // Restrict the player's horizontal movement while still allowing them to fall.
    }

    void DisableHorizontalMovement()
    {
        if (playerRigidbody != null)
        {
            Vector2 currentVelocity = playerRigidbody.velocity;
            currentVelocity.x = 0; // Set horizontal velocity to zero while preserving vertical velocity.
            playerRigidbody.velocity = currentVelocity;
        }
    }

    void EnableHorizontalMovement()
    {
        if (playerRigidbody != null)
        {
            // Restore the player's ability to move horizontally.

        }
    }
    public void Flash()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        
        currentCoroutine = StartCoroutine(StunFlash());
    }
    private IEnumerator StunFlash()
    {
        player1SpriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(stunDuration);

        player1SpriteRenderer.material = originalPlayer1Mat;

        StopCoroutine(StunFlash());
    }
}
