using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
public class Score : MonoBehaviour
{
    [SerializeField]
    ScoreText[] st = new ScoreText[5];
    [SerializeField]
    TMP_InputField input;

    float[] scoreboard = new float[5];
    string[] names = new string[5];
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            Debug.Log(names[i] + " has a score of " + scoreboard[i]);
        }
        DisplayScores();
        
    }
    private void Awake()
    {
        InitialScores();
        int count = 0;
        try
        {
            StreamReader sr = new StreamReader(Application.dataPath + "HighScores.txt");
            string dataline = sr.ReadLine();
            while(dataline != null && count < 5)
            {
                string[] oldscores = dataline.Split(",");
                names[count] = oldscores[0];
                scoreboard[count] = Int32.Parse(oldscores[1]);
                dataline = sr.ReadLine();
                count++;
            }
            sr.Close();
        }
        catch (Exception e)
        {
            Debug.Log("File not found");
            
        }
    }
    void DisplayScores()
    {
        for(int i = 0; i < 5; i++)
        {

            st[i].Display(names[i], scoreboard[i]);
        }
    }
    void InitialScores()
    {
        scoreboard[0] = 0;
        names[0] = "Null";
        scoreboard[1] = 0;
        names[1] = "Null";
        scoreboard[2] = 0;
        names[2] = "Null";
        scoreboard[3] = 0;
        names[3] = "Null";
        scoreboard[4] = 0;
        names[4] = "Null";
    }
    public void SaveScores()
    {
        string newName = input.text;
        float newScore = PlayerPrefs.GetFloat("HighScore");


        int minIndex = -1;
        float minScore = newScore;
        for(int i = 0; i < 5; i++)
        {
            if(scoreboard[i] < minScore)
            {
                minIndex = i;
                minScore = scoreboard[i];
            }

        }

        if(minIndex != -1)
        {
            scoreboard[minIndex] = newScore;
            names[minIndex] = newName;
        }


        //sort scores with names
        for (int i = 0; i < scoreboard.Length - 1; i++)
        {
            // Find the minimum element in unsorted array
            int min_idx = i;
            for (int j = i + 1; j < scoreboard.Length; j++)
                if (scoreboard[j] < scoreboard[min_idx])
                    min_idx = j;

            // Swap the found minimum element with the first
            // element
            float temp = scoreboard[min_idx];
            string temp2 = names[min_idx];
            scoreboard[min_idx] = scoreboard[i];
            names[min_idx] = names[i];
            scoreboard[i] = temp;
            names[i] = temp2;
        }


        DisplayScores();
        try
        {
            StreamWriter sw = new StreamWriter(Application.dataPath + "HighScores.txt");
            for(int i = 0; i < 5; i++)
            {
                sw.WriteLine(names[i] + "," + scoreboard[i]);
            }
            sw.Close();
            
        }catch(Exception e)
        {
            Debug.Log("Could not create file");
        }
    }
    public void LoadScores()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
