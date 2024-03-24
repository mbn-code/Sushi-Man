using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] internal int Score = 0;
    [SerializeField] PlayerController PlyController;
    [SerializeField] Animator AC;
    [SerializeField] Spawner SpawnerController;

    private string sceneToLoad;
    internal void FadeToNext(string SceneName)
    {
        AC.SetTrigger("FadeOut");
        sceneToLoad = SceneName;
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.Save();
        StartCoroutine("LoadNewScene");
    }

    private IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(sceneToLoad);
    }
    internal void Restart()
    {
        FadeToNext("MainMenu");
    }

    internal void Awake()
    {
        if (PlayerPrefs.GetInt("Score") != 0)
        {
            Score = PlayerPrefs.GetInt("Score");
        }

        if (PlayerPrefs.GetInt("HasDash") == 1) 
        {
            PlyController.DashingUnlocked = true;
        }

        if (SceneManager.GetActiveScene().name == "FightArea")
        {
            if(PlayerPrefs.GetInt("Dif") == 0)
            {
                SpawnerController.MaxBalls = 3;
                SpawnerController.spawnTime = 5f;
            } else if(PlayerPrefs.GetInt("Dif") == 1)
            {
                SpawnerController.MaxBalls = 4;
                SpawnerController.spawnTime = 4f;
            } else if (PlayerPrefs.GetInt("Dif") == 2)
            {
                SpawnerController.MaxBalls = 5;
                SpawnerController.spawnTime = 3f;
            } else if (PlayerPrefs.GetInt("Dif") == 3)
            {
                SpawnerController.MaxBalls = 6;
                SpawnerController.spawnTime = 2f;
            } else
            {
                SpawnerController.MaxBalls = 8;
                SpawnerController.spawnTime = 2f;
            }
        }
    }

    internal void AddPoint()
    {
        Score += 1;
    }
}