using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colGO = collision.gameObject;
        if (colGO.tag == "EnemyHead")
        {
            colGO.transform.parent.gameObject.SetActive(false);
        }
    }
}
