using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    private float score = 0; 

    [SerializeField] private Text scoreText; 


    public void UpdateScore(float newY)
    {
        score = newY; 
        UpdateScoreText(); 
    }

    public void AddToScore(int amount)
    {
        score += amount; 
        UpdateScoreText(); 
    }

    // Method to update the score text on the UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F0"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore(transform.position.y);
    }
}

