using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    //Position in the set array
    public int x;
    public int y;
       
    //Connections to other blocks
    public BlockScript up;
    public BlockScript down;
    public BlockScript left;
    public BlockScript right;

    //How big the blocks are
    public float w;
    public float h;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if(boxCollider != null) {
            w = boxCollider.size.x;
            h = boxCollider.size.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
