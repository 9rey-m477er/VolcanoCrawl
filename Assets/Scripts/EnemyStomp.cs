using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    [SerializeField]
    private GameObject floatingPoints;
    AudioSource source;
    public AudioClip rockCrush;

    public void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colGO = collision.gameObject;
        if (colGO.tag == "EnemyHead")
        {
            source.PlayOneShot(rockCrush, 1f);
            ShowPoints(collision.gameObject, "+10");
            colGO.transform.parent.gameObject.SetActive(false);
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
