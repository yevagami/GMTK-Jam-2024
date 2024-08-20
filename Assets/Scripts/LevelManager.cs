using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{

    public List<SetScript> setsInLevel = new List<SetScript>();

    [Header("Level Transition")]
    public string NextLevel = "";
    public Animator BlackScreenTransition;
    public GameObject levelCompleteMessagePrefab;
    public GameObject UICanvas;
    Coroutine LevelTransitionCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelTransitionCoroutine == null) {
            LevelTransitionCoroutine = StartCoroutine(LevelCompleteTransition());
        }
    }

    public void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LevelComplete() {

    }

    IEnumerator LevelCompleteTransition() {
        GameObject msg = Instantiate(levelCompleteMessagePrefab, Vector3.zero, Quaternion.identity, UICanvas.transform);
        if (msg != null) {
            yield return null;
        }

        BlackScreenTransition.Play("BlackScreenFadeOut");
        yield return new WaitForSeconds(BlackScreenTransition.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("NEXT LEVEL");
    }
}
