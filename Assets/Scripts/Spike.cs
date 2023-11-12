using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    private bool isStunned = false;
    private float stunDuration = 1.0f;
    private float stunTimer = 0.0f;

    private Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D component.

    private void Start()
    {
        // Find and store a reference to the player's Rigidbody2D component.
        playerRigidbody = GameObject.FindGameObjectWithTag("Player1").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
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
        if (collision.gameObject.tag == "Player" && !isStunned)
        {
            Debug.Log("Stunned");
            StunPlayer();
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
}
