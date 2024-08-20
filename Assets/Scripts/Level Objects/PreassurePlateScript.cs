using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreassurePlateScript : MonoBehaviour
{
    //Level Manager
    [Header("Level Manager")]
    public LevelManager levelManager;

    [Header("Object To Trigger")]
    public TriggerScriptInterface triggerScript;

    [Header("")]
    public int activationAmount = 1;
    bool isActivated = false;

    // Start is called before the first frame update
    private void Start() {
        if (levelManager == null) {
            levelManager = GameObject.FindGameObjectWithTag("levelManager").GetComponent<LevelManager>();
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (isActivated) {
            triggerScript.Activate();
        } else {
            triggerScript.Deactivate();
        }
    }


    private void OnTriggerStay2D(Collider2D collision) {
        if (levelManager == null) {
            return;
        }

        if (collision.gameObject.tag == "block") {
            SetScript set = collision.gameObject.transform.parent.gameObject.GetComponent<SetScript>();
            if (set == null) {
                return;
            }

            if (set.blocks.Count >= activationAmount) {
                isActivated = true;
                return;
            }

            isActivated = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (levelManager == null) {
            return;
        }

        if (collision.gameObject.tag == "block") {
            isActivated = false;
        }
    }
}
