using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering;

public class SetScript : MonoBehaviour{
    //Blocks
    List<BlockScript> blocks = new List<BlockScript>();
    BlockScript pivot;
    public float spaceBetweenBlocks = 0.3f;
    bool sort = false;
    GameObject blockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GetBlocksFromChildren();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (!sort) {
            AdjustBlockPositions();
            sort = true;    
        }
    }

    //Adding the child blocks into the blocks vector
    public void AdjustBlockPositions() {
        foreach (BlockScript block in blocks) {
            //up
            if (block.up != null) {
                block.gameObject.transform.position = block.up.gameObject.transform.position - new Vector3(0.0f, block.up.h/2 + spaceBetweenBlocks, 0.0f); 
            }

            //Down
            if (block.down != null) {
                block.gameObject.transform.position = block.down.gameObject.transform.position + new Vector3(0.0f, block.down.h / 2 + spaceBetweenBlocks, 0.0f);
            }

            //Left
            if (block.left != null) {
                block.gameObject.transform.position = block.left.gameObject.transform.position + new Vector3(block.left.w / 2 + spaceBetweenBlocks, 0.0f, 0.0f);
            }

            //Right
            if(block.right != null) {
                block.gameObject.transform.position = block.right.gameObject.transform.position - new Vector3(block.right.w / 2 + spaceBetweenBlocks, 0.0f, 0.0f);
            }
            Debug.Log(string.Format("{0} Pos: {1}", block.gameObject.name, transform.position));
        }
    }

    //Retrieving all the blocks from the children
    public void GetBlocksFromChildren() {
        int childrenCount = transform.childCount;
        for(int i = 0; i < childrenCount; i++) {
            BlockScript child = transform.GetChild(i).GetComponent<BlockScript>();
            if(child == null) {
                Debug.Log("Not a valid block");
                continue;
            }
            blocks.Add(child);
        }
    }

    public void GenerateBlocks(int width, int height) {
        if(blockPrefab == null) {
            Debug.Log("blockPrefab not set");
            return; 
        }

        for(int i = 0; i < width; i++) {
            for(int j = 0; j < height; j++) {

                BlockScript newBlock = Instantiate(blockPrefab).GetComponent<BlockScript>();
                blocks.Add(newBlock);

                if (i != 0 && i % width != 0) {
                    newBlock.left = blocks[i - 1];
                }

                if(j != 0) {
                    newBlock.up = blocks[i + (j - 1)];
                }
            } 
        }
    }
}
