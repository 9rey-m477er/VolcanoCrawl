using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Scenes : MonoBehaviour
{
   public void SinglePlayer()
    {
        SceneManager.LoadScene("Platform 1");
    }
   public void TwoPlayer()
    {
        SceneManager.LoadScene("Two Player Mode");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
