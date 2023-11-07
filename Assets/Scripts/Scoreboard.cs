using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    private float scoreC = 0;
    private float scoreY = 0;
    private float score = 0;

    [SerializeField] private Text scoreText;

    public float Score
    {
        get => score;
    }

    void Start()
    {
        float loadedHighScore = PlayerPrefs.GetFloat("HighScore", 0f);
                                                                       
    }
    public void UpdateScore(float newY)
    {
         scoreY = newY;
        UpdateScoreText(); 
    }

    public void AddToScore(int amount)
    {
        scoreC += amount; 
        UpdateScoreText();
        Debug.Log("Score updated. New score: " + scoreC);
    }

    // Method to update the score text on the UI
    private void UpdateScoreText()
    {
        score = scoreC + scoreY;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString("F0"); 
            PlayerPrefs.SetInt("HighScore", (int)score);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        UpdateScore(transform.position.y);
    }
}

