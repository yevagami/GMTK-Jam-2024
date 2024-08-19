using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

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
    enum states { Move, SelectPivot, SelectDetach, Detach, Attach}
    states currentState = states.Move;
    public float inputDelayTime = 0.5f;
    Coroutine selectPivotInput = null;

    //Set management
    public GameObject setPrefab;
    BlockScript currentPivot = null;
    SetScript currentSet = null;

    //Detaching
    Dictionary<BlockScript, BlockScript> selectedBlocksToDetach = new Dictionary<BlockScript, BlockScript>();
    Coroutine selectDetachInput = null;
    BlockScript detachHead = null;

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


        //J Inputs       
        if(Input.GetKeyUp(KeyCode.J)) {
            switch (currentState) {
                case states.Move:
                    currentState = states.SelectPivot;
                    SelectPivotInit();
                    Debug.Log("Selecting Pivot");
                    break;

                case states.SelectPivot:
                    currentState = states.SelectDetach;
                    DetachInit();
                    Debug.Log("Pivot confirmed, select blocks");
                    break;

                case states.Detach:
                    Debug.Log("Detached");
                    break;

                case states.SelectDetach:
                    currentState = states.Detach;
                    break;
            }
        }

        //States update
        switch (currentState) {
            case states.SelectPivot:
                if(selectPivotInput == null) {
                    selectPivotInput = StartCoroutine(SelectPivotInputCoroutine());
                }
                
                break;


            case states.SelectDetach:
                if (selectDetachInput == null) {
                    selectDetachInput = StartCoroutine(SelectDetachInputCoroutine());
                }
                break;


            case states.Detach:
                DetachEnd();
                currentState = states.Move;
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
    //Delaying the result of the input because it's causing a not so good experience
    IEnumerator SelectPivotInputCoroutine() {
        yield return new WaitForSeconds(inputDelayTime);
        SelectPivot();
        selectPivotInput = null;
    }

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
    void DetachInit() {
        selectedBlocksToDetach.Add(currentPivot, null);
        detachHead = currentPivot;
    }

    IEnumerator SelectDetachInputCoroutine() {
        yield return new WaitForSeconds(inputDelayTime);
        SelectDetach();
        selectDetachInput = null;
    }

    void SelectDetach() {
        //Selecting blocks to detach
        //Check if the move is valid
        //if the block has already been selected/unselect it
        //Set that current block as the head
        if(detachHead != null) {
            detachHead.GetComponent<SpriteRenderer>().color = Color.green;
        }

        //Right
        if (moveInput.x == 1) {
            BlockScript right;
            //This move is valid
            if (detachHead.connections.TryGetValue("right", out right)) {
                
                //Check if it's not in the map
                if (selectedBlocksToDetach.TryAdd(right, detachHead)) {
                    detachHead.GetComponent<SpriteRenderer>().color = Color.red;
                    right.GetComponent<SpriteRenderer>().color = Color.red;
                    detachHead = right;
                } 

                //If it's in the map
                // & That specific block has a connection to the head
                //Set that as the head
                else {
                    if(selectedBlocksToDetach.ContainsKey(detachHead)) {
                        if (selectedBlocksToDetach[detachHead] == right) {
                            detachHead.GetComponent<SpriteRenderer>().color = Color.white;
                            selectedBlocksToDetach.Remove(detachHead);
                            detachHead = right;
                        }
                    }
                }

            }
        }

        //Left
        if (moveInput.x == -1) {
            BlockScript left;
            if (detachHead.connections.TryGetValue("left", out left)) {

                if (selectedBlocksToDetach.TryAdd(left, detachHead)) {
                    detachHead.GetComponent<SpriteRenderer>().color = Color.red;
                    left.GetComponent<SpriteRenderer>().color = Color.red;
                    detachHead = left;
                } 
                
                else {
                    if (selectedBlocksToDetach.ContainsKey(detachHead)) {
                        if (selectedBlocksToDetach[detachHead] == left) {
                            detachHead.GetComponent<SpriteRenderer>().color = Color.white;
                            selectedBlocksToDetach.Remove(detachHead);
                            detachHead = left;
                        }
                    }
                }

            }
        }

        //Up
        if (moveInput.y == 1) {
            BlockScript up;
            if (detachHead.connections.TryGetValue("up", out up)) {

                if (selectedBlocksToDetach.TryAdd(up, detachHead)) {
                    detachHead.GetComponent<SpriteRenderer>().color = Color.red;
                    up.GetComponent<SpriteRenderer>().color = Color.red;
                    detachHead = up;
                } 
                
                else {

                    if (selectedBlocksToDetach.ContainsKey(detachHead)) {
                        if (selectedBlocksToDetach[detachHead] == up) {
                            detachHead.GetComponent<SpriteRenderer>().color = Color.white;
                            selectedBlocksToDetach.Remove(detachHead);
                            detachHead = up;
                        }
                    }

                }

            }
        }

        //Down
        if (moveInput.y == -1) {
            BlockScript down;
            if (detachHead.connections.TryGetValue("down", out down)) {

                if (selectedBlocksToDetach.TryAdd(down, detachHead)) {
                    detachHead.GetComponent<SpriteRenderer>().color = Color.red;
                    down.GetComponent<SpriteRenderer>().color = Color.red;
                    detachHead = down;
                } 
                
                else {
                    if (selectedBlocksToDetach.ContainsKey(detachHead)) {
                        if (selectedBlocksToDetach[detachHead] == down) {
                            detachHead.GetComponent<SpriteRenderer>().color = Color.white;
                            selectedBlocksToDetach.Remove(detachHead);
                            detachHead = down;
                        }
                    }
                }

            }
        }


    }

    void DetachEnd() {
        //Creating a new set
        GameObject newSetObject = Instantiate(setPrefab);
        SetScript newSet = newSetObject.GetComponent<SetScript>();

        //Disconnecting the blocks' connections to the previous set
        foreach(KeyValuePair<BlockScript, BlockScript> blockMap in selectedBlocksToDetach) {
            BlockScript currentConnection;

            //If that block is not in the map
            //Disconnect
            if (blockMap.Key.connections.TryGetValue("up", out currentConnection)) {
                if(!selectedBlocksToDetach.ContainsKey(currentConnection)) {
                    blockMap.Key.DisconnectUp();
                }
            }

            if (blockMap.Key.connections.TryGetValue("down", out currentConnection)) {
                if (!selectedBlocksToDetach.ContainsKey(currentConnection)) {
                    blockMap.Key.DisconnectDown();
                }
            }

            if (blockMap.Key.connections.TryGetValue("left", out currentConnection)) {
                if (!selectedBlocksToDetach.ContainsKey(currentConnection)) {
                    blockMap.Key.DisconnectLeft();
                }
            }

            if (blockMap.Key.connections.TryGetValue("right", out currentConnection)) {
                if (!selectedBlocksToDetach.ContainsKey(currentConnection)) {
                    blockMap.Key.DisconnectRight();
                }
            }
        }

        //Adding the selected blocks to the new set
        foreach (KeyValuePair<BlockScript, BlockScript> blockMap in selectedBlocksToDetach) {
            newSet.blocks.Add(blockMap.Key);
        }

        newSetObject.transform.position = newSet.blocks[0].transform.position;

        //Parenting them to the new set
        foreach (BlockScript block in newSet.blocks) {
            block.transform.SetParent(newSetObject.transform);
            block.GetComponent<SpriteRenderer>().color = Color.white;
        }

        //Removing the blocks from the old set
        for(int i = 0; i < currentSet.blocks.Count; i++) {
            if (selectedBlocksToDetach.ContainsKey(currentSet.blocks[i])) {
                currentSet.blocks.RemoveAt(i);
            }
        }

        //Cleanup
        detachHead.GetComponent<SpriteRenderer>().color = Color.white;
        detachHead = null;
        currentPivot = null;
        currentSet = null;
        selectedBlocksToDetach.Clear();

        Possess(newSetObject);
    }

    public void DetachMode() {

    }
}