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

    [SerializeField] private GameObject floatingPoints;
    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
        scoreboard= GetComponent<Scoreboard>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            ShowPoints(collision.gameObject, "+10");
            ParticleSystem ps = collision.gameObject.GetComponentInChildren<ParticleSystem>();
            ps.Play();
            collision.gameObject.GetComponent<Renderer>().enabled = false;
            collision.gameObject.GetComponent<BoxCollider2D>().enabled= false;
            audioSource.PlayOneShot(coinCollectSound, 1f);
            coins++;
            coinText.text = "Crystals: " + coins;

            scoreboard.AddToScore(10);
            //collision.gameObject.SetActive(false);
            Destroy(collision.gameObject, 0.25f);
        }
    }
    void ShowPoints(GameObject crystalObj, string pointText)
    {
        if (floatingPoints)
        {
            GameObject ftPrefab = Instantiate(floatingPoints, crystalObj.transform.position, Quaternion.identity);
            ftPrefab.GetComponentInChildren<TextMesh>().text = pointText;
        }
    }
}
