using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Spawner : MonoBehaviour{

    [SerializeField] private GameObject[] SpawnerList;
    [SerializeField] internal float spawnTime = 2f;
    [SerializeField] private GameObject BallObject;
    [SerializeField] internal int MaxBalls = 1;
    [SerializeField] GameManager GM;

    private float Timer;

    private List<GameObject> Balls = new List<GameObject>();

    private bool isSpawning = true;

    private int SpawnedAmount = 0;

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

        SpawnedAmount++;
        if (SpawnedAmount >= MaxBalls)
        {
            isSpawning = false;
        }

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
        StartCoroutine(SpawnItems(BallObject));
    }

    private void Finish(GameObject BallObject)
    {
        Balls.Remove(BallObject);
        Destroy(BallObject);
        if (Balls.Count == 0 && !isSpawning)
        {
            StartCoroutine("GoToShop");
        }
    }

    private IEnumerator GoToShop()
    {
        yield return new WaitForSeconds(5f);
        GM.FadeToNext("ShopBoy");
    }

    internal void DropItem(GameObject Enemy)
    {
        int MaxDrop = 5;

        switch(PlayerPrefs.GetInt("Dif"))
        {
            case 1:
                MaxDrop = 5;
                break;
            case 2:
                MaxDrop = 10;
                break;
            case 3:
                MaxDrop = 15;
                break;
        }

        int RandomAmount = Random.Range(1, MaxDrop);

        for(int i = 0; i < RandomAmount; i++)
        {
            int randomIndex = Random.Range(0, ItemDrops.Length);
            GameObject itemDrop = ItemDrops[randomIndex];
            Instantiate(itemDrop, Enemy.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
    }

    IEnumerator SpawnItems(GameObject Enemy)
    {
        Vector3 SpawnPos = Enemy.transform.position;
        Enemy.transform.position = new Vector3(1000, 10000, 0);
        int MaxDrop = 5;

        switch (PlayerPrefs.GetInt("Dif"))
        {
            case 1:
                MaxDrop = 5;
                break;
            case 2:
                MaxDrop = 10;
                break;
            case 3:
                MaxDrop = 15;
                break;
        }

        int RandomAmount = Random.Range(1, MaxDrop);

        for (int i = 0; i < RandomAmount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            int randomIndex = Random.Range(0, ItemDrops.Length);
            GameObject itemDrop = ItemDrops[randomIndex];
            Instantiate(itemDrop, SpawnPos + new Vector3(0, 1, 0), Quaternion.identity);
        }

        Finish(Enemy);
    }


}