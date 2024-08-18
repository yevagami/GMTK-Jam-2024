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

    //Detach, Attach, MoveStates
    enum states { Move, SelectPivot ,Detach, Attach}
    states currentState = states.Move;

    //Set management
    public GameObject setPrefab;
    BlockScript currentPivot = null;
    SetScript currentSet = null;

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
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyUp(KeyCode.J)) {
            switch (currentState) {
                case states.Move:
                    currentState = states.SelectPivot;
                    SelectPivotInit();
                    Debug.Log("Selecting Pivot");
                    break;

                case states.SelectPivot:
                    currentState = states.Detach;
                    Debug.Log("Pivot confirmed, select blocks");
                    break;

                case states.Detach:
                    currentState = states.Move;
                    break;
            }
        }

        switch (currentState) {
            case states.SelectPivot:
                SelectPivot();
                break;

            case states.Detach:
                DetachEnd();
                break;
        }


    }


    //Physics related stuff
    private void FixedUpdate() {
        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, currentPawn.transform.position, ref cameraMoveSpeed, cameraDampTime);

        switch (currentState) {
            case states.Move:
                if (currentPawn != null) {
                    currentPawnRB.velocity = moveInput * moveSpeed;
                }
                break;
        }
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

    //Moving the pivot
    void SelectPivotInit() {
        currentSet = currentPawn.GetComponent<SetScript>();
        if (currentSet == null) {
            currentState = states.Move;
            Debug.Log("Pawn is not a set");
            return;
        }

        if (currentPivot == null) {
            currentPivot = currentSet.blocks[0];
            currentPivot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void SelectPivot() {
        //Right
        if(moveInput.x == 1) {
            BlockScript right;
            if (currentPivot.connections.TryGetValue("right", out right)) {
                currentPivot.GetComponent<SpriteRenderer>().color = Color.white;
                currentPivot = right;
                currentPivot.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        //Left
        if (moveInput.x == -1) {
            BlockScript left;
            if (currentPivot.connections.TryGetValue("left", out left)) {
                currentPivot.GetComponent<SpriteRenderer>().color = Color.white;
                currentPivot = left;
                currentPivot.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        //Up
        if (moveInput.y == 1) {
            BlockScript up;
            if (currentPivot.connections.TryGetValue("up", out up)) {
                currentPivot.GetComponent<SpriteRenderer>().color = Color.white;
                currentPivot = up;
                currentPivot.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }

        //Down
        if (moveInput.y == -1) {
            BlockScript down;
            if (currentPivot.connections.TryGetValue("down", out down)) {
                currentPivot.GetComponent<SpriteRenderer>().color = Color.white;
                currentPivot = down;
                currentPivot.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }


    //Detaching from the main set
    void DetachEnd() {
        GameObject newSetObject = Instantiate(setPrefab);
        SetScript newSet = newSetObject.AddComponent<SetScript>();
        newSet.blocks.Add(currentPivot);
        currentPivot.transform.SetParent(newSet.transform);

        //Removing all the connections
        BlockScript currentConnection;
        if (currentPivot.connections.TryGetValue("up", out currentConnection)){
            currentPivot.DisconnectUp();
        }

        if (currentPivot.connections.TryGetValue("down", out currentConnection)) {
            currentPivot.DisconnectDown();
        }

        if (currentPivot.connections.TryGetValue("left", out currentConnection)) {
            currentPivot.DisconnectLeft();
        }

        if (currentPivot.connections.TryGetValue("right", out currentConnection)) {
            currentPivot.DisconnectRight();
        }

        //Removing the block from the block list
        currentSet.blocks.Remove(currentPivot);

        currentPivot = null;
        currentSet = null;

        Possess(newSetObject);
    }

    public void DetachMode() {

    }
}
