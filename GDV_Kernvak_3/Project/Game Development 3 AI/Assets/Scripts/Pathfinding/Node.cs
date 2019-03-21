using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    //X and Y positions in the Node Array
    public int gridX;
    public int gridY;

    //Obstruction of the node
    public bool IsWall;

    //World position of the node
    public Vector3 Position;

    //Previous Node
    public Node Parent;

    //Node Information
    public int gCost;                                   //Cost from start to node
    public int hCost;                                   //Cost from node to end
    public int FCost { get { return gCost + hCost; } }  //Total node cost


    //Node Constructor
    public Node(bool IsWall, Vector3 Pos, int gridX, int gridY)
    {
        this.IsWall = IsWall;
        this.Position = Pos;
        this.gridX = gridX;
        this.gridY = gridY;
    }

}//CLASS
