using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeResetScript : MonoBehaviour{
    //Level Manager
    [Header("Level Manager")]
    public LevelManager levelManager;

    //Respawning Position
    [Header("Respawn Position")]
    public GameObject respawnPos;
    
    // Start is called before the first frame update
    void Start()
    {
        if (levelManager == null) {
            levelManager = GameObject.FindGameObjectWithTag("levelManager").GetComponent<LevelManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(respawnPos == null) {
            return;
        }

        if(levelManager == null) {
            return;
        }

        if(collision.gameObject.tag == "block") {
            SetScript set = collision.gameObject.transform.parent.gameObject.GetComponent<SetScript>();
            if (set == null) {
                return;
            }

            foreach(SetScript s in levelManager.setsInLevel) {
                if(s != set) {
                    Destroy(s.gameObject);
                }
            }

            set.DeleteBlocks();
            set.GenerateSet(3, 3);
            set.transform.position = respawnPos.transform.position;
        }
    }
}
