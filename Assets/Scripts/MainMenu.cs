using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void Play()
    { 
        // Reset Alt
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("HasDash", 0);
        PlayerPrefs.SetInt("Dif", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene("FightArea");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
