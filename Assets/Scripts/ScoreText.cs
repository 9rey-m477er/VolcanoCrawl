using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI highscore;
    // Start is called before the first frame update
    public void Display(string name, float score)
    {
        highscore.text = name + " | " + score;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
