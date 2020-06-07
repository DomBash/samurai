using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    public Transform enemyPrefab;
    private int radius = 18;
    public bool playerAttack = false; //To check if any enemy is attacking player
    public int enemiesToKill = 1;
    private bool tutStart_2 = false;
    private bool tutStart_3 = false;
    private bool tutStart_4 = false;
    public bool roundStart_2 = false;
    public bool roundStart_3 = false;
    public bool roundStart_4 = false;
    public bool roundStart_5 = false;
    public bool roundStart_Final = false;
    public bool winGame = false;
    public bool preGame = true;

    public GameObject enemyHolder;

    public float t = 1f;
    private float duration = 1f;

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

        if (enemiesToKill == 0 && tutStart_2)
            Tutorial_2();
        if (enemiesToKill == 0 && tutStart_3)
            Tutorial_3();
        if (enemiesToKill == 0 && tutStart_4)
            Tutorial_4();

        if (Input.GetKeyDown(KeyCode.T))
        {
            //spawnEnemy();
        }

        if (enemiesToKill == 0)
        {
            systemScript.SetIsReadyForNextRound(true);
            systemScript.SetBGLight();
            systemScript.PlayLightBG();
        }

        if (winGame && enemiesToKill == 0)
        {
            systemScript.WinGame();          
        }

        if (roundStart_Final && enemiesToKill == 1)
        {
            StartCoroutine(Round_Final());
        }

        

        if (systemScript.isDead)
        {
            StopAllCoroutines();
            enemiesToKill = 1;
            systemScript.HideAllTutorialElements();

            preGame = true;
            tutStart_2 = false;
            tutStart_3 = false;
            tutStart_4 = false;
            roundStart_2 = false;
            roundStart_3 = false;
            roundStart_4 = false;
            roundStart_5 = false;
            winGame = false;

        }
    }

    void DarkMood()
    {
        t = 0;
        systemScript.PlayDarkBG();
    }

    public void StartGame()
    {
        
        playerAttack = false;

        StopAllCoroutines();
        enemiesToKill = 1;

        preGame = true;
        tutStart_2 = false;
        tutStart_3 = false;
        tutStart_4 = false;
        roundStart_2 = false;
        roundStart_3 = false;
        roundStart_4 = false;
        roundStart_5 = false;
        winGame = false;
    }

    public void RestartGame()
    {
        playerAttack = false;

        StopAllCoroutines();
        enemiesToKill = 1;

        preGame = true;
        tutStart_2 = false;
        tutStart_3 = false;
        tutStart_4 = false;
        roundStart_2 = false;
        roundStart_3 = false;
        roundStart_4 = false;
        roundStart_5 = false;
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
    }

    public IEnumerator StartT1()
    {
        enemiesToKill = 1;
        yield return new WaitForSeconds(5f);       
        DarkMood();
        
        spawnEnemy();
        systemScript.ShowTutorial1Text();
        tutStart_2 = true;
    }

    void Tutorial_2()
    {
        tutStart_2 = false;
        enemiesToKill = 1;
        spawnEnemy();
        systemScript.ShowTutorial2Text();
        tutStart_3 = true;
    }

    void Tutorial_3()
    {
        tutStart_3 = false;
        enemiesToKill = 1;
        spawnEnemy();
        systemScript.ShowTutorial3Text();
        tutStart_4 = true;
    }

    void Tutorial_4()
    {
        tutStart_4 = false;
        systemScript.HideAllTutorialElements();
        roundStart_2 = true;
    }

    public IEnumerator StartR2()
    {
        roundStart_2 = false;
        print("Round 2 started");
        enemiesToKill = 4;
        systemScript.ShowRound2Text();

        yield return new WaitForSeconds(5f);
        DarkMood();
        systemScript.HideAllTutorialElements();
        systemScript.HideTutorial();
        

        spawnEnemy();
        yield return new WaitForSeconds(5f);
        spawnEnemy();
        yield return new WaitForSeconds(5f);
        spawnEnemy();
        yield return new WaitForSeconds(5f);
        spawnEnemy();

        roundStart_3 = true;
    }

    public IEnumerator StartR3()
    {
        roundStart_3 = false;
        print("Round 3 started");
        enemiesToKill = 4;
        yield return new WaitForSeconds(5f);
        DarkMood();

        spawnEnemy();
        spawnEnemy();
        yield return new WaitForSeconds(4f);
        spawnEnemy();
        spawnEnemy();

        roundStart_4 = true;
    }

    public IEnumerator StartR4()
    {
        roundStart_4 = false;
        print("Round 4 started");
        enemiesToKill = 6;
        yield return new WaitForSeconds(5f);
        DarkMood();

        spawnEnemy();
        spawnEnemy();
        spawnEnemy();
        yield return new WaitForSeconds(3f);

        spawnEnemy();
        spawnEnemy();
        spawnEnemy();

        roundStart_5 = true;
    }

    public IEnumerator StartR5()
    {
        roundStart_5 = false;
        print("Round 5 started");
        enemiesToKill = 6;
        yield return new WaitForSeconds(5f);
        DarkMood();

        spawnEnemy();
        spawnEnemy();
        spawnEnemy();
        spawnEnemy();
        spawnEnemy();

        roundStart_Final = true;
    }

    public IEnumerator Round_Final()
    {
        roundStart_Final = false;
        print("Final started");
        enemiesToKill = 20;
        yield return new WaitForSeconds(5f);

        for (int i = 0; i < 20; i++)
        {
            spawnEnemy();   
            yield return new WaitForSeconds(0.1f);
        }
        systemScript.SetCanUseSpecial(true);
        systemScript.ShowFinalRoundText();
        winGame = true;
    }

    void spawnEnemy()
    {
        var a = UnityEngine.Random.value * (2 * Mathf.PI) - Mathf.PI;
        var x = Mathf.Cos(a) * radius;
        var z = Mathf.Sin(a) * radius;
        Vector3 position = new Vector3(x, 2.58f, z);
        var newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        newEnemy.transform.parent = enemyHolder.transform;
    }
}
