using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour{

    [SerializeField] private GameObject[] SpawnerList;
    [SerializeField] private float spawnTime = 2f;
    [SerializeField] private GameObject BallObject;
    
    internal int MaxBalls = 1;

    private float Timer;

    private List<GameObject> Balls = new List<GameObject>();

    private bool isSpawning = true;

    [SerializeField]
    private GameObject[] ItemDrops;

    void Update() {
        Timer += Time.deltaTime;
        if(Timer >= spawnTime && isSpawning && Balls.Count < MaxBalls)
        {
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

    internal void RemoveBall(GameObject BallObject)
    {
        DropItem(BallObject);
        Balls.Remove(BallObject);
        Destroy(BallObject);
    }

    internal void DropItem(GameObject Enemy)
    {
        int RandomAmount = Random.Range(1, 2);

        for(int i = 0; i < RandomAmount; i++)
        {
            int randomIndex = Random.Range(0, ItemDrops.Length);
            GameObject itemDrop = ItemDrops[randomIndex];
            Instantiate(itemDrop, Enemy.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }
}