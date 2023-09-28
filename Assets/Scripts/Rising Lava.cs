using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RisingLava : MonoBehaviour
{
    private float riseSpeed = .1f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncreaseRiseSpeed());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Dead");
            Die();
        }
    }
    // Update is called once per frame
    void Update()
    {
        rb.position += new Vector2(rb.velocity.x, (riseSpeed * Time.deltaTime));
        
    }
   void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator IncreaseRiseSpeed()
    {
        yield return new WaitForSeconds(1); // Wait for 1 second before increasing

        while (riseSpeed < 1.0f)
        {
            riseSpeed += 0.1f; // Increase riseSpeed by 0.1
            yield return new WaitForSeconds(1); // Wait for 1 second before the next increase
        }
    }
}
