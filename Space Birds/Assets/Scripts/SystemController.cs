using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemController : MonoBehaviour
{

    public GameObject meteorPrefab;
    public GameObject meteorMiddlePrefab;
    public Transform meteorHolder;
    private float meteorRate = 0.5f;
    private float nextMeteorTime = 0f;
    


    public UIController ui;
    public PlayerMovement player;

    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            var meteor = Instantiate(meteorPrefab, new Vector2( -8f + (i * 1.5f), 4.5f), Quaternion.identity);
            meteor.transform.SetParent(meteorHolder);
        }

        for (int i = 0; i < 15; i++)
        {
            var meteor = Instantiate(meteorPrefab, new Vector2( -8f + (i * 1.5f), -4.5f), Quaternion.identity);
            meteor.transform.SetParent(meteorHolder);
        }
    }

    void Update()
    {
        if(Time.time >= nextMeteorTime)
        {
            SpawnMeteor();
            nextMeteorTime = Time.time + 1f / meteorRate;
        }
    }

    void SpawnMeteor()
    {
        var meteor = Instantiate(meteorMiddlePrefab, new Vector2(12f, Random.Range(-4f, 4f)), Quaternion.identity);
        meteor.transform.SetParent(meteorHolder);
    }

    public void GameOver()
    {
        ui.GameOverMenu();
    }

    public void Replay()
    {
        player.Replay();
        print("replaying");
    }

    public void Play()
    {
        print("system play");
    }

}
