using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BTNode
{
    //Node Stats
    private BTNode[] nodes;

    //Constructor
    public Sequence(params BTNode[] nodes)
    {
        this.nodes = nodes;
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Sequence Nodes
        foreach (BTNode n in nodes)
        {
            TaskStatus status = n.Run();
            switch (status)
            {
                case TaskStatus.Completed: continue;
                case TaskStatus.Failed: return TaskStatus.Failed;
                case TaskStatus.Running: return TaskStatus.Running;
            }
        }
        return TaskStatus.Completed;
    }

}//CLASS
