using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollector : MonoBehaviour
{
    private int coins = 0;
    [SerializeField] private Text coinText;
     Scoreboard scoreboard;

    public AudioClip coinCollectSound;
    AudioSource audioSource;
    

    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
        scoreboard= GetComponent<Scoreboard>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(coinCollectSound, 1f);
            coins++;
            coinText.text = "Crystals: " + coins;

            scoreboard.AddToScore(10);
            collision.gameObject.SetActive(false);
        }
    }
}
