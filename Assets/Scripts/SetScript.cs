using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering;

public class SetScript : MonoBehaviour{
    //Blocks
    BlockScript[,] blocks;
    Dictionary<BlockScript, BlockScript[]> blockScripts; //Map of connections
    public float spaceBetweenBlocks = 0.3f;
    public GameObject blockPrefab;
    int setWidth;
    int setHeight;  

    // Start is called before the first frame update
    void Start()
    {
        GenerateSet(4, 4);
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustBlockPositions();
    }

    //Adding the child blocks into the blocks vector
    public void AdjustBlockPositions() {

        //The pivot will always be the first block
        BlockScript pivot = blocks[0,0];
        pivot.transform.position = Vector3.zero;


        foreach (BlockScript block in blocks) {
            if (block == pivot) {
                continue;
            };

            //up
            if (block.up != null) {
                block.up.transform.position = block.transform.position + new Vector3(0.0f, block.h/2 + spaceBetweenBlocks, 0.0f); 
            }

            //Down
            if (block.down != null) {
                block.down.transform.position = block.transform.position - new Vector3(0.0f, block.h / 2 + spaceBetweenBlocks, 0.0f);
            }

            //Left
            if (block.left != null) {
                block.left.transform.position = block.gameObject.transform.position - new Vector3(block.w / 2 + spaceBetweenBlocks, 0.0f, 0.0f);
            }

            //Right
            if(block.right != null) {
                block.right.transform.position = block.gameObject.transform.position + new Vector3(block.w / 2 + spaceBetweenBlocks, 0.0f, 0.0f);
            }
        }
    }

    public void GenerateSet(int width, int height) {
        if(blockPrefab == null) {
            Debug.Log("blockPrefab not set");
            return; 
        }

        //Creating the set dimensions
        setWidth = width;
        setHeight = height;

        //Creating the blocks
        blocks = new BlockScript[width, height];
        for(int j = 0; j < height; j++) {
            for(int i = 0; i < width; i++) {
                BlockScript newBlock = Instantiate(blockPrefab).GetComponent<BlockScript>();
                newBlock.transform.parent = transform;
                newBlock.gameObject.name = string.Format("{0} {1}", "Block", i + j * width);
                blocks[i,j] = newBlock;
                newBlock.x = i;
                newBlock.y = j;
            } 
        }

        //Attaching the blocks to one another
        for (int j = 0; j < height; j++) {
            for (int i = 0; i < width; i++) {
                //Left
                if(i - 1 >= 0) {
                    blocks[i, j].left = blocks[i - 1, j];
                }

                //Right
                if(i + 1 < width) {
                    blocks[i,j].right = blocks[i + 1, j];
                }

                //Up
                if(j - 1 >= 0) {
                    blocks[i, j].up = blocks[i, j - 1];
                }

                //Down
                if(j + 1 < height) {
                    blocks[i, j].down = blocks[i, j + 1];
                }
            }
        }
    }
}
