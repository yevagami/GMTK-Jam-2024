using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {
    [Header("DEBUGGING")]
    //DEBUG CONNECTIONS ARRAY
    public BlockScript[] DEBUGCONNECTIONS = new BlockScript[4];

    void DRAWCONNECTIONS() {
        foreach(KeyValuePair<string, BlockScript> c in connections) {
            if(c.Value == null) {
                continue;
            }
            Color lineColor = Color.black;
            Vector3 offset = Vector3.zero;

            if (c.Key == "up") {
                lineColor = Color.green;
                offset = new Vector3(-1.0f, 0.0f, 0.0f);
            }
            if (c.Key == "down") {
                lineColor = Color.red;
                offset = new Vector3(1.0f, 0.0f, 0.0f);
            }
            if (c.Key == "left") {
                lineColor = Color.blue;
                offset = new Vector3(0.0f, 1.0f, 0.0f);
            }
            if (c.Key == "right") {
                lineColor = Color.yellow;
                offset = new Vector3(0.0f, -1.0f, 0.0f);
            }

            offset *= 0.08f;
            Debug.DrawLine(transform.position + offset, c.Value.transform.position + offset, lineColor);
        }
    }

    [Header("")]
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

    //FOR DEBUGGING ONLY
    private void FixedUpdate() {
        BlockScript foundConnections;
        connections.TryGetValue("up", out foundConnections);
        DEBUGCONNECTIONS[0] = foundConnections;

        connections.TryGetValue("down", out foundConnections);
        DEBUGCONNECTIONS[1] = foundConnections;

        connections.TryGetValue("left", out foundConnections);
        DEBUGCONNECTIONS[2] = foundConnections;

        connections.TryGetValue("right", out foundConnections);
        DEBUGCONNECTIONS[3] = foundConnections;

        DRAWCONNECTIONS();
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
            leftObject.connections.Remove("right");
            connections.Remove("left");
            return;
        }
        return;
    }

    public void DisconnectRight() {
        BlockScript rightObject;
        if (connections.TryGetValue("right", out rightObject)) {
            rightObject.connections.Remove("left");
            connections.Remove("right");
            return;
        }
        return;
    }

    public void DisconnectUp() {
        BlockScript upObject;
        if (connections.TryGetValue("up", out upObject)) {
            upObject.connections.Remove("down");
            connections.Remove("up");
            return;
        }
        return;
    }

    public void DisconnectDown() {
        BlockScript downObject;
        if (connections.TryGetValue("down", out downObject)) {
            downObject.connections.Remove("up");
            connections.Remove("down");
            return;
        }
        return;
    }
}
