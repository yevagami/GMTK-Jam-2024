using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
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
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame() {
        levelManager.ExitGame();
    }

}
