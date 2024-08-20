using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    //Level Manager
    [Header("Level Manager")]
    public LevelManager levelManager;

    private void Start() {
        if (levelManager == null) {
            levelManager = GameObject.FindGameObjectWithTag("levelManager").GetComponent<LevelManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(levelManager == null){
            return;
        }
        
        if(collision.gameObject.tag == "block") {
            SetScript set = collision.gameObject.transform.parent.gameObject.GetComponent<SetScript>();
            if (set == null) {
                return;
            }
            levelManager.ResetLevel();
        }
    }
}
