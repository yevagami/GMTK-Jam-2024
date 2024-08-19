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
        if(collision.gameObject.tag == "block") {
        }
    }
}
