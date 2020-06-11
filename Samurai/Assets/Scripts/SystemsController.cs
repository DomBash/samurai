using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsController : MonoBehaviour
{
    private bool isPaused = true;
    private bool isDead = false;
    private bool canPause = false;

    public Transform cam;
    public Transform ui;
    public Transform spawner;
    public Transform sound;
    public Transform player;
    public Transform animator;
    public Transform treeTouch;

    public Transform soulHolder;
    public Transform soulPrefab;
    public Transform enemyHolder;
    public Transform enemyPrefab;
    public Transform bossPrefab;
    public Transform boss;
    public GameObject finalAttackEffect;
    public ParticleSystem treeStartEffect;

    private CameraController camScript;
    private UIController uiScript;
    private EnemySpawner spawnScript;
    private CharacterMovement playerScript;
    private MixerFixer audioScript;
    private AnimationController animScript;
    private TreeTouch treeScript;

    private string[] controllerNames;
    private List<Transform> enemies = new List<Transform>();

    public bool isControllerTutorial = false;
    private bool gameStarted = false;
    private bool canUseSpecial = false;
    private bool isWaitingForHit = false;
    private bool isPlayerPowered = false;
    private bool isReadyForNextRound = true;
    private bool isTouchingTree = false;
    private bool isBeingAttacked = false;
    private bool isFinalAttacking = false;
    private bool isHA = false;
    private bool isLA = false;
    private bool isAtBoss = false;

    private int nextRound = 1;
    private int numSouls = 0;
    private int enemiesToKill = 0;

    void Start()
    {
        Cursor.visible = false;
        camScript = cam.GetComponent<CameraController>();
        uiScript = ui.GetComponent<UIController>();
        spawnScript = spawner.GetComponent<EnemySpawner>();
        audioScript = sound.GetComponent<MixerFixer>();
        playerScript = player.GetComponent<CharacterMovement>();
        animScript = animator.GetComponent<AnimationController>();
        treeScript = treeTouch.GetComponent<TreeTouch>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            audioScript.PlayEnemyDeathAudio();
        }

            if (Application.targetFrameRate != 60) //Set FPS
            Application.targetFrameRate = 60;

        controllerNames = Input.GetJoystickNames();

        if (controllerNames.Length > 0) //Controller check
        {
            if (controllerNames[0] != "" && !isControllerTutorial)
            {
                uiScript.ControllerTutorialOn();
                playerScript.contDashUp = false;

                isControllerTutorial = true;
            }
            else if (controllerNames[0] == "" && isControllerTutorial)
            {
                uiScript.ControllerTutorialOff();
                isControllerTutorial = false;
            }
        }

        if (isPaused)
        {
            if (!isControllerTutorial)
                Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        if (Input.GetButtonDown("Pause")) //ESCAPE or Start
        {
            if(isControllerTutorial)
                playerScript.contDashUp = false;
            if (isDead)
            {
                camScript.SetBGLight();  

                uiScript.OpenMainMenu();
                isDead = false;
                isAtBoss = false;
                spawnScript.inTutorial = false;
                isPlayerPowered = false;
                MoveSpiritsToTree();
                animScript.SetWalkAnim(false);
            }
            else if (!canPause && uiScript.optionsMenu.activeSelf)
            {
                gameStarted = false;
                uiScript.OpenMainMenu();
            }
            else if (!canPause && uiScript.mainMenu.activeSelf)
            {
                StartGame();
            }
            else if (canPause)
            {
                isPaused = true;
                uiScript.OpenPauseMenu();
                canPause = false;
            }           
            else if (!canPause)
            {
                ResumeGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) //ESCAPE or Start
        {
            MoveSpiritsToPlayer();
        }

    }

    public void StartGame()
    {       
        uiScript.StartGame();
        camScript.StartGame();
        audioScript.StartGame();

        MoveSpiritsToTree();

        isPaused = false;
        gameStarted = true;
        canPause = true;
        canUseSpecial = false;
        finalAttackEffect.SetActive(false);
        isReadyForNextRound = true;

        isDead = false;
        isBeingAttacked = false;
        numSouls = 0;
        enemiesToKill = 0;
    }

    public void ResumeGame()
    {
        isPaused = false;
        canPause = true;
        uiScript.HideAllMenus();
        camScript.ResumeGame();
        uiScript.ShowTutorial();
    }

    public void RestartGame()
    {
        DestroyAllEnemies();
        DestroyAllSouls();
        if (boss != null)
        {
            DestroyHealthTicks();
            Destroy(boss.gameObject);
            DestroyAllSpikes();
        }
        DestroyAllSpikes();
        
        uiScript.RestartGame();
        playerScript.RestartGame();
        spawnScript.RestartGame();
        camScript.RestartGame();
        audioScript.RestartGame();

        if (isAtBoss)
            nextRound = 7;
        else
            nextRound = 1;

        isDead = false;
        isPaused = false;
        MoveSpiritsToTree();
        gameStarted = true;
        canPause = true;
        isBeingAttacked = false;
        numSouls = 0;
        enemiesToKill = 0;
        isReadyForNextRound = true;

    }

    //Get Bools ---------------------------------------------

    public bool GetGameStarted()
    {
        return gameStarted;
    }

    public bool GetCanUseSpecial()
    {
        return canUseSpecial;
    }

    public bool GetIsWaitingForHit()
    {
        return isWaitingForHit;
    }

    public bool GetIsPlayerPowered()
    {
        return isPlayerPowered;
    }

    public bool GetIsTouchingTree()
    {
        return isTouchingTree;
    }

    public bool GetIsBeingAttacked()
    {
        return isBeingAttacked;
    }

    public bool GetIsFinalAttacking()
    {
        return isFinalAttacking;
    }

    public bool GetIsHA()
    {
        return isHA;
    }

    public bool GetIsLA()
    {
        return isLA;
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public bool GetIsAtBoss()
    {
        return isAtBoss;
    }

    //Set Bools ----------------------------------------------

    public void SetCanUseSpecial(bool parity)
    {
        canUseSpecial = parity;
        if (parity)
            finalAttackEffect.SetActive(true);
    }

    public void SetIsWaitingForHit(bool parity)
    {
        isWaitingForHit = parity;

        SetEnemiesForHit(parity);
    }

    public void SetIsPlayerPowered(bool parity)
    {
        isPlayerPowered = parity;
    }

    public void SetIsTouchingTree(bool parity)
    {
        isTouchingTree = parity;
    }

    public void SetIsBeingAttacked(bool parity)
    {
        isBeingAttacked = parity;
    }

    public void SetIsFinalAttacking(bool parity)
    {
        isFinalAttacking = parity;
    }

    public void SetIsHA(bool parity)
    {
        isHA = parity;
    }

    public void SetIsLA(bool parity)
    {
        isLA = parity;
    }

    public void SetIsAtBoss(bool parity)
    {
        isAtBoss = parity;
    }

    //Get Ints --------------------------------------------------

    public int GetNumSouls()
    {
        return numSouls;
    }

    public int GetEnemiesToKill()
    {
        return enemiesToKill;
    }

    public int GetNextRound()
    {
        return nextRound;
    }

    //Set Ints --------------------------------------------------

    public void SetNumSouls(int num)
    {
        numSouls = num;
    }

    public void SetNextRound(int num)
    {
        nextRound = num;
    }

    public void SetEnemiesToKill(int num)
    {
        enemiesToKill = num;
    }

    //Animations ----------------------------------------------

    public void SetAnimSpeed(float speed)
    {
        animScript.SetAnimSpeed(speed);
    }

    public void SetTreeTouchAnim(bool parity)
    {
        animScript.SetTreeTouchAnim(parity);
        StartCoroutine(TreeEffect());
        audioScript.PlayTreeTouchAudio();
    }

    IEnumerator TreeEffect()
    {
        yield return new WaitForSeconds(0.8f);
        treeStartEffect.Play();
    }

    public void SetLAAnim(bool parity)
    {
        animScript.SetLAAnim(parity);
    }

    public void SetHAAnim(bool parity)
    {
        animScript.SetHAAnim(parity);
    }

    public void SetDashAnim(bool parity)
    {
        animScript.SetDashAnim(parity);
    }

    public void SetWalkAnim(bool parity)
    {
        animScript.SetWalkAnim(parity);
    }

    public void SetFinalAttackAnim(bool parity)
    {
        animScript.SetFinalAttackAnim(parity);
    }

    //Rounds ------------------------------------------------

    public bool GetIsReadyForNextRound()
    {
        return isReadyForNextRound;
    }

    public void SetIsReadyForNextRound(bool parity)
    {
        isReadyForNextRound = parity;
    }

    public void StartNextRound()//Player collision with tree
    {        
        playerScript.TouchTree();
        isTouchingTree = true;
        isReadyForNextRound = false;

        if (nextRound == 4)
            MoveSpiritsToPlayer();

        spawnScript.StartNextRound(nextRound);
        nextRound += 1;
    }

    public void DestroyAllEnemies()
    {
        GameObject[] currEnemies;
        currEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in currEnemies)
        {
            Destroy(enemy);
        }
        enemies = new List<Transform>();
    }

    public void DestroyAllSouls()
    {
        GameObject[] souls;
        souls = GameObject.FindGameObjectsWithTag("Soul");
        foreach (GameObject soul in souls)
        {
            Destroy(soul);
        }
    }

    public void DestroyAllSpikes()
    {
        GameObject[] spikes;
        spikes = GameObject.FindGameObjectsWithTag("Spiker");
        foreach (GameObject spike in spikes)
        {
            Destroy(spike);
        }
    }

    public void DestroyHealthTicks()
    {
        if (boss.GetComponent<BossController>().healthTicks.Count != 0)
        {
            foreach (Transform tick in boss.GetComponent<BossController>().healthTicks)
            {
                Destroy(tick.gameObject);
            }
            boss.GetComponent<BossController>().healthTicks = new List<Transform>();
        }
    }

    public void PlayDarkBG()
    {
        audioScript.PlayDarkBG();
    }

    public void PlayLightBG()
    {
        audioScript.PlayLightBG();
    }

    public void SetBGLight()
    {
        camScript.SetBGLight();
    }

    public void SetBGDark()
    {
        camScript.SetBGDark();
    }

    public void LerpBGDark(float t)
    {
        camScript.LerpBGDark(t);       
    }

    public void HideAllTutorialElements()
    {
        uiScript.HideAllTutorialElements();
    }

    public void ShowTutorial1Text()
    {
        uiScript.ShowTutorial1Text();
    }

    public void ShowTutorial2Text()
    {
        uiScript.ShowTutorial2Text();
    }

    public void ShowRound2Text()
    {
        uiScript.ShowRound2Text();
    }

    void MoveSpiritsToPlayer()
    {
        isPlayerPowered = true;
        soulHolder.parent = GetPlayerTransform();
        soulHolder.localPosition = new Vector3(0f, 4f, 0f);
        playerScript.speed = 10.0f;
    }

    void MoveSpiritsToTree()
    {
        isPlayerPowered = false;
        soulHolder.parent = GetTreeTransform();
        soulHolder.localPosition = new Vector3(0f, 6f, 0f);
        playerScript.speed = 6.0f;
    }

    public void ShowFinalRoundText()
    {
        uiScript.ShowFinalRoundText();
    }

    public void ShowCheckpointTut()
    {
        uiScript.ShowCheckpointTut();
    }

    public void SpawnBoss()
    {
        boss = Instantiate(bossPrefab, new Vector3(10f, 6.68f, 10f), Quaternion.identity);
        audioScript.PlayBossSpawnAudio();
    }

    public void EnemyDeath(Transform thisEnemy)
    {
        enemiesToKill -= 1;
        audioScript.PlayEnemyDeathAudio();
        foreach (Transform enemy in enemies)
        {
            if (enemy.transform == thisEnemy)
            {
                enemies.Remove(enemy);
                break;
            }
        }
        spawnScript.EnemyDeath();
    }

    //Audio ------------------------------------------------------

    public void PlaySword1Audio()
    {
        audioScript.PlaySword1Audio();
    }

    public void PlayHeavyAudio()
    {
        audioScript.PlayHeavyAudio();
    }

    public void PlayDashAudio()
    {
        audioScript.PlayDashAudio();
    }

    public void PlayEnemyAttackAudio()
    {
        audioScript.PlayEnemyAttackAudio();
    }

    public void PlayDeathAudio()
    {
        audioScript.PlayDeathAudio();
    }

    public void PlayBossDeathAudio()
    {
        audioScript.PlayBossDeathAudio();
    }

    public void PlayHitAudio()
    {
        audioScript.PlayHitAudio();
    }

   /*public void PlayEnemyDeathAudio()
    {
        audioScript.PlayEnemyDeathAudio();
    }*/

    //Dev Tools ----------------------------------------------------

    public void SpawnSoul(Vector3 pos)
    {
        numSouls += 1;
        var newSoul = Instantiate(soulPrefab, pos, Quaternion.identity);
        newSoul.transform.parent = soulHolder.transform;
    }

    public void SpawnEnemy(Vector3 pos)
    {
        if(enemiesToKill == 0)
            enemiesToKill += 1;
        var newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        newEnemy.transform.parent = enemyHolder.transform;
        enemies.Add(newEnemy);
    }

    void SetEnemiesForHit(bool parity)
    {
        foreach(Transform enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().isWaitingForHit = parity;
        }
    }

    public Transform GetCamTransform()
    {
        return camScript.GetCamTransform();
    }

    public Transform GetPlayerTransform()
    {
        return playerScript.GetPlayerTransform();
    }

    public Transform GetTreeTransform()
    {
        return treeScript.GetTreeTransform();
    }

    public void Dead(bool isTreeDeath)
    {
        print("Death");
        isPaused = true;
        isDead = true;

        if(isControllerTutorial)
            playerScript.contDashUp = false;

        uiScript.Dead(isTreeDeath);
        playerScript.Dead();

        DestroyAllEnemies();
        DestroyAllSouls();
        if (boss != null)
        {
            DestroyHealthTicks();
            Destroy(boss.gameObject);
            DestroyAllSpikes();
           
        }

        PlayDeathAudio();
    }

    public void WinGame()
    {

        DestroyAllEnemies();
        //Destroy(boss.gameObject);
        SetBGLight();
        PlayLightBG();
        SetIsReadyForNextRound(true);
        uiScript.WinGame();
    }

    public void ExitGame()
    {
        Application.Quit();
        print("Exit Game");
        //UnityEditor.EditorApplication.isPlaying = false;
    }


}
