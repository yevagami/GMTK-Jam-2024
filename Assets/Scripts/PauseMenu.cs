using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public PlayerControllerScript playerControllerScript;
    public GameObject AudioMenu;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAudioMenu() {
        AudioMenu.SetActive(true);
    }

    public void CloseAudioMenu() {
        AudioMenu.SetActive(false);
    }

    public void ClosePauseMenu() {
        Time.timeScale = 1;
        playerControllerScript.gamePaused = false;
        gameObject.SetActive(false);
    }

    public void ExitGame() {
        ClosePauseMenu();
        levelManager.ExitGame();
    }

}
