using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemCollector : MonoBehaviour
{
    private int coins = 0;
    [SerializeField] private Text coinText;

    public AudioClip coinCollectSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            audioSource.PlayOneShot(coinCollectSound, 1f);
            Destroy(collision.gameObject);
            coins++;
            coinText.text = "Coins: " + coins;
        }
    }
}
