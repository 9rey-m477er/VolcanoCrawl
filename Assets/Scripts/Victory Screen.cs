using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    string winner = "";
    public TMP_Text win;
    // Start is called before the first frame update
    void Start()
    {
        winner = PlayerPrefs.GetString("winner");
        if (winner.Equals("Player1"))
        {
            win.text = "Player 2 wins!";
        }
        else if (winner.Equals("Player2"))
        {
            win.text = "Player 1 wins!";
        }
        else
        {
            win.text = "Who won?";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
