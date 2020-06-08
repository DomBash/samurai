using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemsController : MonoBehaviour
{
    public bool isPaused = true;
    public bool isDead = false;
    public bool canPause = false;

    public Transform cam;
    public Transform ui;
    public Transform spawner;
    public Transform sound;
    public Transform player;
    public Transform animator;
    public Transform treeTouch;

    private CameraController camScript;
    private UIController uiScript;
    private EnemySpawner spawnScript;
    private CharacterMovement playerScript;
    private MixerFixer audioScript;
    private AnimationController animScript;
    private TreeTouch treeScript;

    private string[] controllerNames;
    private bool isControllerTutorial = false;
    private bool gameStarted = false;
    public bool canUseSpecial = false;
    private bool isWaitingForHit = false;
    private bool isPlayerPowered = false;
    private bool isReadyForNextRound = true;
    private bool isTouchingTree = false;
    private bool isBeingAttacked = false;
    private bool isFinalAttacking = false;
    private bool isHA = false;
    private bool isLA = false;

    private int nextRound = 1;

    void Start()
    {
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
        if (isPaused) //Hide cursor when not in menu
            Cursor.visible = true;
        else
            Cursor.visible = false;

        if (Application.targetFrameRate != 60) //Set FPS
            Application.targetFrameRate = 60;

        controllerNames = Input.GetJoystickNames();

        if (controllerNames.Length > 0) //Controller check
        {
            if (controllerNames[0] != "" && !isControllerTutorial)
            {
                uiScript.ControllerTutorialOn();
                isControllerTutorial = true;
            }
            else if (controllerNames[0] == "" && isControllerTutorial)
            {
                uiScript.ControllerTutorialOff();
                isControllerTutorial = false;
            }
        }

        if (Input.GetButtonDown("Pause")) //ESCAPE or Start
        {
            if (isDead)
            {
                camScript.SetBGLight();  
                uiScript.OpenMainMenu();
                isDead = false;
            }
            else if (!canPause && uiScript.optionsMenu.activeSelf)
            {
                gameStarted = false;
                uiScript.OpenMainMenu();
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

    }

    public void StartGame()
    {       
        uiScript.StartGame();
        camScript.StartGame();
        audioScript.StartGame();

        isPaused = false;
        gameStarted = true;
        canPause = true;
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

        uiScript.StartGame();
        playerScript.RestartGame();
        spawnScript.RestartGame();
        camScript.RestartGame();
        audioScript.RestartGame();

        isDead = false;
        isPaused = false;
        gameStarted = true;
        canPause = true;
        nextRound = 1;
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

    //Set Bools ----------------------------------------------

    public void SetCanUseSpecial(bool parity)
    {
        canUseSpecial = parity;
    }

    public void SetIsWaitingForHit(bool parity)
    {
        isWaitingForHit = parity;
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

    //Animations ----------------------------------------------

    public void SetAnimSpeed(float speed)
    {
        animScript.SetAnimSpeed(speed);
    }

    public void SetTreeTouchAnim(bool parity)
    {
        animScript.SetTreeTouchAnim(parity);
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
        isReadyForNextRound = false;
        spawnScript.StartNextRound(nextRound);
        nextRound += 1;
    }

    public void DestroyAllEnemies()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        
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

    public void HideTutorial()
    {
        uiScript.HideTutorial();
    }

    public void ShowTutorial1Text()
    {
        uiScript.ShowTutorial1Text();
    }

    public void ShowTutorial2Text()
    {
        uiScript.ShowTutorial1Text();
    }

    public void ShowTutorial3Text()
    {
        uiScript.ShowTutorial1Text();
    }

    public void ShowRound2Text()
    {
        uiScript.ShowRound2Text();
    }

    public void ShowFinalRoundText()
    {
        uiScript.ShowFinalRoundText();
    }

    public void EnemyDeath()
    {
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

    public Transform GetCamTransform()
    {
        return camScript.GetCamTransform();
    }

    public Transform GetPlayerTransform()
    {
        return playerScript.GetPlayerTransform();
    }

    public void Dead(bool isTreeDeath)
    {
        print("Death");
        isPaused = true;
        isDead = true;
        uiScript.Dead(isTreeDeath);
        playerScript.Dead();

        DestroyAllEnemies();
        DestroyAllSouls();
        
        PlayDeathAudio();
    }

    public void WinGame()
    {
        uiScript.WinGame();
    }

    public void ExitGame()
    {
        Application.Quit();
        print("Exit Game");
        //UnityEditor.EditorApplication.isPlaying = false;
    }


}
