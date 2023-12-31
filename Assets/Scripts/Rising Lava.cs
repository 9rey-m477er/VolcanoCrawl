using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RisingLava : MonoBehaviour
{
    private float riseSpeed = .1f;
    public float resetDistance = 15f; // Distance above the player to reset to
    public Rigidbody2D rb;
    [SerializeField] Transform player;
    private Vector2 initialPosition; // Store the initial position of the lava

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncreaseRiseSpeed());
        initialPosition = rb.position; // Store the initial position
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1"|| collision.gameObject.tag == "Player2")
        {
            PlayerPrefs.SetString("winner", collision.gameObject.tag);
            Debug.Log("Dead");
            Die();
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.position += new Vector2(rb.velocity.x, (riseSpeed * Time.deltaTime));

        // Check the distance between the lava and the player
        float distanceToPlayer = Mathf.Abs(player.position.y - rb.position.y);
        if (distanceToPlayer > resetDistance)
        {
            // If the distance is greater than resetDistance, snap back to the initial position
            SnapBackToInitialPosition();
        }
    }

    void Die()
    {
        if(SceneManager.GetActiveScene().name.Equals("Two Player Mode"))
        {

            SceneManager.LoadScene("Victory Screen");
        }
        else
        {
            SceneManager.LoadScene("LeaderBoard");
        }
        
    }

    IEnumerator IncreaseRiseSpeed()
    {
        yield return new WaitForSeconds(1); // Wait for 1 second before increasing

        while (riseSpeed < 0.7f)
        {
            riseSpeed += 0.05f; // Increase riseSpeed by 0.05
            yield return new WaitForSeconds(1); // Wait for 1 second before the next increase
        }
    }

    void SnapBackToInitialPosition()
    {
        rb.position = new Vector2(rb.position.x, player.position.y - resetDistance);
    }
}
