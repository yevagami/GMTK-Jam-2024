using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {
    //Position in the set array
    public int x;
    public int y;
    
    //A dictionary of connections
    public Dictionary<string, BlockScript> connections = new Dictionary<string, BlockScript>();

    //A dictionary of their directions
    public Dictionary<string, Vector3> directions = new Dictionary<string, Vector3>(){
        {"up", Vector3.up },
        {"down", Vector3.down },
        {"left", Vector3.left },
        {"right", Vector3.right }
    };


    //Connections to other blocks
    public BlockScript up;
    public BlockScript down;
    public BlockScript left;
    public BlockScript right;

    //How big the blocks are
    public float w;
    public float h;

    private void Awake() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null) {
            w = boxCollider.size.x;
            h = boxCollider.size.y;
        }
    }
    
    //Methods for connecting blocks
    public void ConnectUp(BlockScript objectAbove) {
        if (!connections.TryAdd("up", objectAbove)) {
            connections["up"] = objectAbove;
        };

        if (!objectAbove.connections.TryAdd("down", this)) {
            objectAbove.connections["down"] = this;
        };
    }

    public void ConnectDown(BlockScript objectBelow) {
        if (!connections.TryAdd("down", objectBelow)) {
            connections["down"] = objectBelow;
        };

        if (!objectBelow.connections.TryAdd("up", this)) {
            objectBelow.connections["up"] = this;
        };
    }

    public void ConnectLeft(BlockScript leftObject) {
        if (!connections.TryAdd("left", leftObject)) {
            connections["left"] = leftObject;
        };

        if (!leftObject.connections.TryAdd("right", this)) {
            leftObject.connections["right"] = this;
        };
    }

    public void ConnectRight(BlockScript rightObject) {
        if (!connections.TryAdd("right", rightObject)) {
            connections["right"] = rightObject;
        };

        if (!rightObject.connections.TryAdd("left", this)) {
            rightObject.connections["left"] = this;
        };
    }

    //Methods for disconnecting blocks
    public void DisconnectLeft() {
        BlockScript leftObject;
        if (connections.TryGetValue("left", out leftObject)){
            leftObject.connections["right"] = null;
            connections["left"] = null;
            return;
        }
        return;
    }

    public void DisconnectRight() {
        BlockScript rightObject;
        if (connections.TryGetValue("right", out rightObject)) {
            rightObject.connections["left"] = null;
            connections["right"] = null;
            return;
        }
        return;
    }

    public void DisconnectUp() {
        BlockScript upObject;
        if (connections.TryGetValue("up", out upObject)) {
            upObject.connections["down"] = null;
            connections["up"] = null;
            return;
        }
        return;
    }

    public void DisconnectDown() {
        BlockScript downObject;
        if (connections.TryGetValue("down", out downObject)) {
            downObject.connections["up"] = null;
            connections["down"] = null;
            return;
        }
        return;
    }
}
