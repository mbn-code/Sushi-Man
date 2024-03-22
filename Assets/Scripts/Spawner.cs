using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour{

    [SerializeField] private GameObject[] SpawnerList;
    [SerializeField] private float spawnTime = 2f;
    [SerializeField] private GameObject BallObject;

    private float Timer;

    private List<GameObject> Balls = new List<GameObject>();

    private bool isSpawning = true;

    void Update() {
        Timer += Time.deltaTime;
        if(Timer >= spawnTime && isSpawning){
            Timer = 0;
            SpawnBall();
        }
    }

    void SpawnBall() {
        int randomIndex = Random.Range(0, SpawnerList.Length);
        GameObject spawner = SpawnerList[randomIndex];
        Vector3 spawnPosition = spawner.transform.position;
        Balls.Add(Instantiate(BallObject, spawnPosition, Quaternion.identity));
    }

    public void DeleteAllBalls() {
        isSpawning = false;
        foreach(GameObject ball in Balls){
            Destroy(ball);
        }
    }
}