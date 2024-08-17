using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    //Input
    Vector2 moveInput;

    //Camera
    public Camera playerCamera;
    public float cameraDampTime;
    Vector3 cameraMoveSpeed;

    //Movement
    public float moveSpeed;

    //Pawn possesion
    GameObject currentPawn;
    Rigidbody2D currentPawnRB;
    [SerializeField] GameObject defaultPawn;

    // Start is called before the first frame update
    void Start()
    {
        Possess(defaultPawn);
        if(playerCamera == null) {
            playerCamera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update(){
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
    }


    //Physics related stuff
    private void FixedUpdate() {
        if(currentPawn != null) {
            currentPawnRB.velocity = moveInput * moveSpeed;
        }
        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, currentPawn.transform.position, ref cameraMoveSpeed, cameraDampTime);
    }


    public void Possess(GameObject newPawn) { 
        if(newPawn == null) { return; }
        currentPawnRB = newPawn.GetComponent<Rigidbody2D>();     
        if(currentPawnRB == null) {
            Debug.Log("Cannot possess an object without a rigidBody component");
            return;
        }
        currentPawn = newPawn;
    }
}
