using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    [SerializeField]
    private GameObject floatingPoints;
    AudioSource source;
    public AudioClip rockCrush;
    [SerializeField]
    private GameObject deathParticle;
    Scoreboard score;

    public void Start()
    {
        source = GetComponent<AudioSource>();
        score = GetComponent<Scoreboard>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colGO = collision.gameObject;
        if (colGO.tag == "EnemyHead")
        {
            source.PlayOneShot(rockCrush, 1f);
            ShowPoints(colGO, "+10");
            DeathFX(colGO);
            colGO.transform.parent.gameObject.SetActive(false);
            score.AddToScore(10);
        }
    }
    void ShowPoints(GameObject enemyObj, string pointText)
    {
        if (floatingPoints)
        {
            GameObject ftPrefab = Instantiate(floatingPoints, enemyObj.transform.position, Quaternion.identity);
            ftPrefab.GetComponentInChildren<TextMesh>().text = pointText;
        }
    }
    void DeathFX(GameObject enemyObj)
    {
        if (deathParticle)
        {
            GameObject deathPart = Instantiate(deathParticle, enemyObj.transform.position, Quaternion.identity);
        }
    }

}
