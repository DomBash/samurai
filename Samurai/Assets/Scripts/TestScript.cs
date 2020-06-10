using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform system;
    private SystemsController systemScript;
    public Transform spawner;
    private EnemySpawner spawnScript;

    void Start()
    {
        systemScript = system.GetComponent<SystemsController>();
        spawnScript = spawner.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SpawnSouls(1);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            systemScript.SpawnBoss();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            systemScript.SpawnEnemy(new Vector3(0f, 2.58f, -15f));
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            systemScript.SetCanUseSpecial(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spawnScript.inTutorial = false;
            systemScript.SetNextRound(2);
            SpawnSouls(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spawnScript.inTutorial = false;
            systemScript.SetNextRound(3);
            SpawnSouls(7);

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            spawnScript.inTutorial = false;
            systemScript.SetNextRound(4);
            SpawnSouls(11);

        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            spawnScript.inTutorial = false;
            systemScript.SetNextRound(5);
            SpawnSouls(17);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            spawnScript.inTutorial = false;
            systemScript.SetNextRound(6);
            systemScript.SetCanUseSpecial(true);
            SpawnSouls(22);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            print("Set next round to Boss...");
            spawnScript.inTutorial = false;
            systemScript.SetNextRound(7);
            systemScript.HideTutorial();
            systemScript.SetCanUseSpecial(true);
            systemScript.SetIsPlayerPowered(true);
            SpawnSouls(0);
        }

        

       
    }

    void SpawnSouls(int num)
    {
        for(int i = 0; i < num; i++)
        {
            systemScript.SpawnSoul(new Vector3(0f, 0f, -15f));
        }
    }
}
