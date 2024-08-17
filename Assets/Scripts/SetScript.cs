using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering;

public class SetScript : MonoBehaviour{
    //Blocks
    BlockScript[,] blocks;
    List<BlockScript>blockScripts = new List<BlockScript>(); //Map of connections
    public float spaceBetweenBlocks = 0.3f;
    public GameObject blockPrefab;
    int setWidth;
    int setHeight;  

    // Start is called before the first frame update
    void Start()
    {
        GenerateSet(3, 2);
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustBlockPositions();
    }

    //Adding the child blocks into the blocks vector
    public void AdjustBlockPositions() {

        //The pivot will always be the first block
        BlockScript pivot = blockScripts[0];
        pivot.transform.position = Vector3.zero;

        foreach (BlockScript block in blockScripts) {
            if (block == pivot) {
                continue;
            };

            foreach(KeyValuePair<string, BlockScript> connection in block.connections) {
                connection.Value.transform.position = block.transform.position + (block.directions[connection.Key] * (block.h / 2 + spaceBetweenBlocks));
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

                blockScripts.Add(newBlock);


                if(i != 0) {
                    newBlock.ConnectLeft(blockScripts[(i + j * width) - 1]);
                }
                
                if(j != 0) {
                    newBlock.ConnectUp(blockScripts[(i + j * width) - width]);
                }
                

                //newBlock.x = i;
                //newBlock.y = j;
            } 
        }

        ////Attaching the blocks to one another
        //for (int j = 0; j < height; j++) {
        //    for (int i = 0; i < width; i++) {
        //        //Left
        //        if(i - 1 >= 0) {
        //            blocks[i, j].left = blocks[i - 1, j];
        //        }

        //        //Right
        //        if(i + 1 < width) {
        //            blocks[i,j].right = blocks[i + 1, j];
        //        }

        //        //Up
        //        if(j - 1 >= 0) {
        //            blocks[i, j].up = blocks[i, j - 1];
        //        }

        //        //Down
        //        if(j + 1 < height) {
        //            blocks[i, j].down = blocks[i, j + 1];
        //        }
        //    }
        //}
    }
}
