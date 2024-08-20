using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{

    public List<SetScript> setsInLevel = new List<SetScript>();

    [Header("Level Transition")]
    public ExitZoneScript exitZone; 
    public string NextLevel = "";
    public Animator BlackScreenTransition;
    public GameObject levelCompleteMessagePrefab;
    public GameObject UICanvas;
    Coroutine LevelTransitionCoroutine = null;
    GameInstanceScript gameInstance;

    // Start is called before the first frame update
    void Start()
    {
        gameInstance = GameObject.FindGameObjectWithTag("gameInstance").GetComponent<GameInstanceScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(exitZone != null) {
            if(exitZone.exitValid) {
                LevelComplete();
            }
        }
    }

    public void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelComplete() {
        if(LevelTransitionCoroutine == null) {
            LevelTransitionCoroutine = StartCoroutine(LevelCompleteTransition());
        }
    }

    IEnumerator LevelCompleteTransition() {
        if(gameInstance != null) {
            gameInstance.PlayVictorySoundEffect();
        }

        GameObject msg = Instantiate(levelCompleteMessagePrefab, Vector3.zero, Quaternion.identity, UICanvas.transform);
        if (msg != null) {
            yield return null;
        }

        BlackScreenTransition.Play("BlackScreenFadeOut");
        yield return new WaitForSeconds(BlackScreenTransition.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene(NextLevel);
    }

    public void ExitGame() {
        StartCoroutine(ExitCoroutine());
    }

    IEnumerator ExitCoroutine() {        
        BlackScreenTransition.Play("BlackScreenFadeOut");
        yield return new WaitForSeconds(BlackScreenTransition.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene("MainMenuScene");
    }
}
