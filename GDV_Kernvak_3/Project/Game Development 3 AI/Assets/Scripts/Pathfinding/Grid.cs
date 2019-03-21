using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    //Start position of the pathfinder
    public Transform StartPosition;
    //Mask of obstructions
    public LayerMask WallMask;
    public bool CheckWalls = true;
    //Size of the grid
    public Vector2 gridWorldSize;
    //the size of each node
    public float nodeRadius;
    //the distance between nodes
    public float Distance;

    //Array of all nodes
    Node[,] grid;
    //The complete path
    public List<Node> FinalPath;

    //Twice the amount of nodeRadius
    float nodeDiameter;
    //Size of the grid in array units
    int gridSizeX, gridSizeY;


    //Run when the program starts
    private void Start()
    {
        //calculate node diameter
        nodeDiameter = nodeRadius * 2;
        //Divide the grid world co-ordinates
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        //Draw grid
        CreateGrid();
    }

    //Run every frame
    private void Update()
    {
        if (CheckWalls) { CheckWallNodes(); }
    }

    //Create Grid with Nodes
    void CreateGrid()
    {
        //Declare array of nodes
        grid = new Node[gridSizeX, gridSizeY];
        //Set the origin of the grid
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        //Loop through the array of nodes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                
                //Set wall nodes
                bool Wall = true;
                if(Physics.CheckSphere(worldPoint, nodeRadius, WallMask))
                {
                    Wall = false;
                }

                //Create a new node in the array
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    //Check the IsWall state of all Nodes
    public void CheckWallNodes()
    {
        //Set the origin of the grid
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        //Loop through the array of nodes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                //Set wall nodes
                bool Wall = true;
                if (Physics.CheckSphere(worldPoint, nodeRadius, WallMask))
                {
                    Wall = false;
                }

                //Re-assign Wall Nodes
                grid[x, y].IsWall = Wall;
            }
        }
    }

    //Get closest node from world position
    public Node NodeFromWorldPosition(Vector3 WorldPos)
    {
        float xpoint = ((WorldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float ypoint = ((WorldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        xpoint = Mathf.Clamp01(xpoint);
        ypoint = Mathf.Clamp01(ypoint);

        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint);
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint);

        return grid[x, y];
    }

    //Get neightboring nodes of the given node
    public List<Node> GetNeighboringNodes(Node node)
    {
        //List of all available neighbors
        List<Node> NeightboringNodes = new List<Node>();
        //Check variables for if the neightboring node is in range to avoid out of range errors
        int xCheck;
        int yCheck;

        //Right Side
        xCheck = node.gridX + 1;
        yCheck = node.gridY;
        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if(yCheck >= 0 && yCheck < gridSizeY)
            {
                NeightboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Left Side
        xCheck = node.gridX - 1;
        yCheck = node.gridY;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeightboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Top Side
        xCheck = node.gridX;
        yCheck = node.gridY + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeightboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Bottom Side
        xCheck = node.gridX;
        yCheck = node.gridY - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                NeightboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return NeightboringNodes;
    }

    //Draws wireframe
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            foreach(Node node in grid)
            {
                if (node.IsWall)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;             
                }

                if (FinalPath != null)
                {
                    if (FinalPath.Contains(node))
                    {
                        Gizmos.color = Color.green;
                    }                
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter - Distance));
            }
        }
    }

    //Get Random Node
    public Node GetRandomNode()
    {
        Node RandomNode = null;

        if (grid != null)
        {
            bool isWall = false;
            while (!isWall)
            {
                int X = Random.Range(0, gridSizeX);
                int Y = Random.Range(0, gridSizeY);

                RandomNode = grid[X, Y];
                isWall = RandomNode.IsWall;
            }
        }

        return RandomNode;
    }

}//CLASS
