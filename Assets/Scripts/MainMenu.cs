using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject BlackScreen;
    public string startSceneName;
    public GameObject AudioSettingsPanel;

    public void StartButton() {
        BlackScreen.GetComponent<Animator>().Play("BlackScreenFadeOut");
        StartCoroutine(TransitionLevel());
    }

    IEnumerator TransitionLevel() {
        yield return new WaitForSeconds(BlackScreen.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        SceneManager.LoadScene(startSceneName);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAudioManager() {
        AudioSettingsPanel.SetActive(true);
    }

    public void CloseAudioManager() {
        AudioSettingsPanel.SetActive(false);
    }
}
