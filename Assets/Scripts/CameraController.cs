using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    private void Update()
    {
            // Single player mode - follow Player1
            if (player2 == null)
            {
                transform.position = new Vector3(transform.position.x, player1.position.y, transform.position.z);
            }
        
        else
        {
            // Two-player mode - stay between Player1 and Player2
            float averageY = (player1.position.y + player2.position.y) / 2f;
            transform.position = new Vector3(transform.position.x, averageY, transform.position.z);
        }
    }
}
