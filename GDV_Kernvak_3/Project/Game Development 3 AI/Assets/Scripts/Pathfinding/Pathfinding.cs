using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    //The World Grid
    public Grid grid;
    //Begin and End Positions
    public Transform StartPosition;
    public Transform TargetPosition;

    //The complete path
    public List<Node> FinalPath;

    //Initalization
    private void Awake()
    {
        //Find World Grid
        grid = GetComponent<Grid>();
        if(grid == null)
        {
            grid = GameObject.FindGameObjectWithTag("AIGrid").GetComponent<Grid>();
        }
    }

    //Find Path
    public void FindPath(Vector3 StartPos, Vector3 TargetPos)
    {
        //Get the start and end node
        Node StartNode = grid.NodeFromWorldPosition(StartPos);
        Node TargetNode = grid.NodeFromWorldPosition(TargetPos);

        //Create Open and Closed Node lists
        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        //Begin of OpenList
        OpenList.Add(StartNode);

        //Find Final Path
        while(OpenList.Count > 0)
        {
            //Find most efficient Node
            Node CurrentNode = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if(OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }

            //Set node to ClosedList
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            //When the most efficient Node is the end node
            if(CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            //Get next neightbor nodes
            foreach(Node NeightborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                //if neightbor node is a wall then ignore it
                if(!NeightborNode.IsWall || ClosedList.Contains(NeightborNode))
                {
                    continue;              
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeightborNode);

                //Find next most efficient node
                if(MoveCost < NeightborNode.gCost || !OpenList.Contains(NeightborNode))
                {
                    NeightborNode.gCost = MoveCost;
                    NeightborNode.hCost = GetManhattenDistance(NeightborNode, TargetNode);
                    NeightborNode.Parent = CurrentNode;

                    //Add potential nodes
                    if (!OpenList.Contains(NeightborNode))
                    {
                        OpenList.Add(NeightborNode);
                    }
                }
            }

        }
    }

    //Get the final path
    void GetFinalPath(Node StartingNode, Node EndNode)
    {
        //Create complete path
        List<Node> CompletePath = new List<Node>();
        Node CurrentNode = EndNode;

        //Go back through total path
        while(CurrentNode != StartingNode)
        {
            CompletePath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        //Reverse path
        CompletePath.Reverse();

        //Set Final Paths
        grid.FinalPath = CompletePath;
        FinalPath = CompletePath;
    }

    //Get the Manhatten Distance
    int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }

}//CLASS
