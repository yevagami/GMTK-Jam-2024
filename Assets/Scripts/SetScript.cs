using System.Collections.Generic;
using UnityEngine;

public class SetScript : MonoBehaviour{
    //Blocks
    public List<BlockScript>blocks = new List<BlockScript>(); //Map of connections
    public float spaceBetweenBlocks = 0.3f;
    public GameObject blockPrefab;
    public bool generateNewSet = false;

    //Legs
    //Legs are the very bottom of the blocks. They are used to check if the block is touching the ground
    List<BlockScript> legs = new();
    public bool IsGrounded = false;
    public float groundCheckDistance = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        if(generateNewSet) {
            GenerateSet(4, 2);
        }
    }

    // Update is called once per frame
    void Update(){
    
    }

    private void FixedUpdate() {
        GroundCheck();
        DEBUGDRAWGROUNDCHECK();
    }

    //Adding the child blocks into the blocks vector
    public void AdjustBlockPositions() {
        if(blocks.Count <= 0) {
            Debug.Log("No blocks to adjust");
            return;
        }

        //The pivot will always be the first block
        BlockScript pivot = blocks[0];

        if (blocks.Count <= 1) {
            Debug.Log("There are only 1 bloks");    
            return;
        }

        foreach (BlockScript block in blocks) {
            if (block == pivot) {
                continue;
            };

            foreach(KeyValuePair<string, BlockScript> connection in block.connections) {
                //Note
                //Because the block.directions[connection.key] is invalid
                if (connection.Value == null) { continue; }
                connection.Value.transform.position = block.transform.position + (block.directions[connection.Key] * (block.h / 2 + spaceBetweenBlocks));
                
                
            }
        }
    }

    public void GenerateSet(int width, int height) {
        if(blockPrefab == null) {
            Debug.Log("blockPrefab not set");
            return; 
        }

        //Creating the blocks
        for(int j = 0; j < height; j++) {
            for(int i = 0; i < width; i++) {
                BlockScript newBlock = Instantiate(blockPrefab).GetComponent<BlockScript>();
                newBlock.transform.parent = transform;
                newBlock.gameObject.name = string.Format("{0} {1}", "Block", i + j * width);

                blocks.Add(newBlock);


                if(i != 0) {
                    newBlock.ConnectLeft(blocks[(i + j * width) - 1]);
                }
                
                if(j != 0) {
                    newBlock.ConnectUp(blocks[(i + j * width) - width]);
                }
                
            } 
        }

        AdjustBlockPositions();
        SetLegs();
    }

    public void SetLegs() {
        legs.Clear();
        float lowestYPos = 0.0f;
        foreach(BlockScript b in blocks) {
            if(b.transform.localPosition.y < lowestYPos) {
                lowestYPos = b.transform.localPosition.y;
                legs.Clear();
                legs.Add(b);
                continue;
            }

            if(b.transform.localPosition.y == lowestYPos) {
                legs.Add(b);
            }
        }
    }

    //Ground check checks for objects with the tag "floor"
    void GroundCheck() {
        IsGrounded = false;
        foreach (BlockScript b in legs) {
            RaycastHit2D[] hit = Physics2D.RaycastAll(b.transform.position - new Vector3(0.0f, b.w/2 + 0.0001f, 0.0f), Vector2.down, groundCheckDistance);
            
            foreach(RaycastHit2D h in hit) {
                if(h.collider.gameObject.tag == "floor" || h.collider.gameObject.tag == "block") {
                    //Debug.Log(string.Format("Y value:{0} Collided at: {1}", b.transform.position.y, h.collider.gameObject.transform.position));
                    Debug.Log("Touched a floor");
                    IsGrounded = true;
                    return;
                }
            }
        }
    }

    void DEBUGDRAWGROUNDCHECK() {
        foreach (BlockScript b in legs) {
            Debug.DrawRay(b.transform.position - new Vector3(0.0f, b.w / 2 + 0.0001f, 0.0f), Vector2.down * groundCheckDistance, Color.magenta);
        }
    }
}
