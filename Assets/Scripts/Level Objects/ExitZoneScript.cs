using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZoneScript : MonoBehaviour
{
    //Level Manager
    [Header("Level Manager")]
    public LevelManager levelManager;

    [Header("Sets in the exit zone")]
    public Dictionary<SetScript, int> setsInExitZone = new();

    [Header("Exit Check")]
    public bool exitValid = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (levelManager == null) {
            levelManager = GameObject.FindGameObjectWithTag("levelManager").GetComponent<LevelManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        SetScript set = collision.gameObject.transform.parent.gameObject.GetComponent<SetScript>();
        if (set != null) { 
            if(!setsInExitZone.ContainsKey(set)) {
                setsInExitZone.Add(set, 0);
            }
        }

        ExitCheck();
        Debug.Log(exitValid);
    }

    void ExitCheck() {
        foreach (SetScript s in levelManager.setsInLevel) {
            if (!setsInExitZone.ContainsKey(s)) {
                exitValid = false;
                return;
            }
        }

        exitValid = true;
    }
}
