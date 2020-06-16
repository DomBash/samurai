using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject gameOverMenu;
    public GameObject mainMenu;
    public GameObject tapPlay;

    public SystemController system;
    public PlayerMovement player;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void ReplayButton()
    {
        system.Replay();
        tapPlay.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    public void PlayButton()
    {
        system.Play();
        tapPlay.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void MainMenuButton()
    {
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void TapPlay()
    {
        tapPlay.SetActive(false);
        player.TapToPlay();
    }
}
