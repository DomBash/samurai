using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject pauseMenu;
    public GameObject tutorial;
    public GameObject deathScreen;

    public GameObject playButton;
    public GameObject resumeButton;
    public GameObject backButton;
    public GameObject deathButton;

    public GameObject laTut;
    public GameObject haTut;
    public GameObject dashTut;
    public GameObject camTut;
    public GameObject soulTut;
    public GameObject treeTut;
    public GameObject dieTut;
    public GameObject specialTut;


    public GameObject winText;
    public GameObject deathText;

    private EventSystem eventSystem;

    public Transform system;
    private SystemsController systemScript;


    void Start()
    {
        eventSystem = EventSystem.current;
        systemScript = system.GetComponent<SystemsController>();

        OpenMainMenu();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            specialTut.SetActive(false);
    }

    public void StartGame()
    {
        HideAllMenus();
        ShowTutorial();
        HideAllTutorialElements();
        treeTut.SetActive(true);
        dashTut.SetActive(true);
    }

    public void RestartGame()//Same as Start Game
    {
        HideAllMenus();
        ShowTutorial();
        HideAllTutorialElements();
        treeTut.SetActive(true);
        dashTut.SetActive(true);
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);

        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        tutorial.SetActive(false);
        deathScreen.SetActive(false);

        eventSystem.SetSelectedGameObject(playButton);
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);

        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        tutorial.SetActive(false);
        deathScreen.SetActive(false);

        eventSystem.SetSelectedGameObject(backButton);
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);

        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        tutorial.SetActive(false);
        deathScreen.SetActive(false);

        eventSystem.SetSelectedGameObject(resumeButton);
    }

    public void OptionsBackButton()
    {
        if (systemScript.GetGameStarted())
            OpenPauseMenu();
        else
            OpenMainMenu();
    }

    public void HideAllMenus() //Doesn't hide tutorial
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        deathScreen.SetActive(false);
    }

    public void ShowTutorial()
    {
        tutorial.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorial.SetActive(false);
    }

    public void HideAllTutorialElements()
    {
        laTut.SetActive(false);
        dashTut.SetActive(false);
        haTut.SetActive(false);
        camTut.SetActive(false);
        soulTut.SetActive(false);
        treeTut.SetActive(false);
        dieTut.SetActive(false);
        specialTut.SetActive(false);
    }

    public void ShowTutorial1Text()
    {
        HideAllTutorialElements();

        laTut.SetActive(true);        
    }

    public void ShowTutorial2Text()
    {
        HideAllTutorialElements();

        haTut.SetActive(true);
        camTut.SetActive(true);
    }

    public void ShowTutorial3Text()
    {
        HideAllTutorialElements();

        soulTut.SetActive(true);
    }

    public void ShowRound2Text()
    {
        HideAllTutorialElements();

        dieTut.SetActive(true);
    }

    public void ShowFinalRoundText()
    {
        HideAllTutorialElements();

        specialTut.SetActive(true);
    }

    public void ControllerTutorialOn()
    {
        laTut.GetComponent<Text>().text = "Press X for light attack";
        haTut.GetComponent<Text>().text = "Press Y for heavy attack";
        dashTut.GetComponent<Text>().text = "LEFT STICK to move \nA to dash";
        camTut.GetComponent<Text>().text = "RIGHT STICK to move camera";
        specialTut.GetComponent<Text>().text = "Press B for a little help with all these enemies";       
    }

    public void ControllerTutorialOff()
    {
        laTut.GetComponent<Text>().text = "LEFT CLICK for light attack";
        haTut.GetComponent<Text>().text = "RIGHT CLICK for heavy attack";
        dashTut.GetComponent<Text>().text = "WASD to move \nSPACE to dash";
        camTut.GetComponent<Text>().text = "MIDDLE MOUSE to move camera";
        specialTut.GetComponent<Text>().text = "Press E for a little help with all these enemies";
    }

    public void Dead(bool treeDeath)
    {
        deathScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(deathButton);

        if (treeDeath)
            deathText.GetComponent<Text>().text = "The Tree Died";
        else
            deathText.GetComponent<Text>().text = "You Died";
    }

    public void WinGame()
    {
        winText.SetActive(true);
    }
}