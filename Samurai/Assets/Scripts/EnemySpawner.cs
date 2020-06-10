using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    public Transform enemyPrefab;
    private int radius = 18;
    //public int enemiesToKill = 1;
    public bool winGame = false;
    public bool inTutorial = true;
    private int nextTutorial = 2;

    public GameObject enemyHolder;

    public float t = 1f;
    private float duration = 1f;
    private float spawnDelay = 4f;

    public Transform system;
    private SystemsController systemScript;

    void Start()
    {
        systemScript = system.GetComponent<SystemsController>();       
    }

    void Update()
    {      
        if (t < 1)
        {
            systemScript.LerpBGDark(t);
            t += Time.deltaTime / duration;
        }

        /*if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemy();
        }*/
    }

    void DarkMood()
    {
        t = 0;
        systemScript.PlayDarkBG();
    }

    public void RestartGame()
    {
        StopAllCoroutines();

        nextTutorial = 2;
        print("shoudl be in tutoiral");
        //inTutorial = true;
        winGame = false;
    }

    public void StartNextRound(int nextRound)
    {
        if (nextRound == 1)
            StartCoroutine(StartT1());
        else if (nextRound == 2)
            StartCoroutine(StartR2());
        else if (nextRound == 3)
            StartCoroutine(StartR3());
        else if (nextRound == 4)
            StartCoroutine(StartR4());
        else if (nextRound == 5)
            StartCoroutine(StartR5());
        else if (nextRound == 6)
            StartCoroutine(StartRF());
        else if (nextRound == 7)
            StartCoroutine(StartRBoss());
    }

    public IEnumerator StartT1()
    {
        print("Starting Tutorial 1...");
        yield return new WaitForSeconds(spawnDelay);       
        DarkMood();
        
        SpawnEnemy();
        systemScript.ShowTutorial1Text();
        //systemScript.SetEnemiesToKill(1);
    }

    void StartT2()
    {
        SpawnEnemy();
        systemScript.ShowTutorial2Text();
        nextTutorial = 3;
    }

    void StartT3()
    {
        SpawnEnemy();
        systemScript.ShowTutorial3Text();
        inTutorial = false;

    }

    public IEnumerator StartR2()
    {
        print("Starting Round 2...");

        systemScript.ShowRound2Text();

        yield return new WaitForSeconds(spawnDelay);
        DarkMood();
        systemScript.HideAllTutorialElements();
        systemScript.HideTutorial();
        

        SpawnEnemy();
        yield return new WaitForSeconds(5f);
        SpawnEnemy();
        yield return new WaitForSeconds(5f);
        SpawnEnemy();
        yield return new WaitForSeconds(5f);
        SpawnEnemy();
    }

    public IEnumerator StartR3()
    {
        print("Starting Round 3...");

        yield return new WaitForSeconds(spawnDelay);
        DarkMood();

        SpawnEnemy();
        SpawnEnemy();
        yield return new WaitForSeconds(4f);
        SpawnEnemy();
        SpawnEnemy();

    }

    public IEnumerator StartR4()
    {
        print("Starting Round 4...");

        yield return new WaitForSeconds(spawnDelay);
        DarkMood();

        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
        yield return new WaitForSeconds(3f);

        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
    }

    public IEnumerator StartR5()
    {
        print("Starting Round 5...");

        yield return new WaitForSeconds(spawnDelay);
        DarkMood();

        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
        SpawnEnemy();
    }

    public IEnumerator StartRF()
    {
        print("Starting Final Round...");

        yield return new WaitForSeconds(spawnDelay);
        DarkMood();

        for (int i = 0; i < 22; i++)
        {
            SpawnEnemy();   
            yield return new WaitForSeconds(0.0f);
        }
        systemScript.SetCanUseSpecial(true);
        systemScript.ShowFinalRoundText();
    }

    public IEnumerator StartRBoss()
    {
        print("Starting Boss Round...");

        yield return new WaitForSeconds(spawnDelay);
        DarkMood();

        systemScript.SpawnBoss();
        winGame = true;
    }

    void StartNextTutorial()
    {
        if (inTutorial)
        {
            if (nextTutorial == 2)
                StartT2();
            else if (nextTutorial == 3)
                StartT3();
        }
    }

    public void EnemyDeath()
    {
        //enemiesToKill -= 1;
        if (systemScript.GetEnemiesToKill() == 0)
        {
            if (inTutorial)
            {
                StartNextTutorial();
            }
            else
            {
                if (winGame)
                    print("win game");//systemScript.WinGame();
                else
                {
                    systemScript.SetIsReadyForNextRound(true);
                    systemScript.SetBGLight();
                    systemScript.PlayLightBG();
                    systemScript.HideAllTutorialElements();
                }
            }
        }
    }

    void SpawnEnemy()
    {
        var a = UnityEngine.Random.value * (2 * Mathf.PI) - Mathf.PI;
        var x = Mathf.Cos(a) * radius;
        var z = Mathf.Sin(a) * radius;
        Vector3 position = new Vector3(x, 2.58f, z);

        systemScript.SpawnEnemy(position);
    }
}
