using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BTNode
{
    //Node Stats
    private BTNode[] nodes;

    //Constructor
    public Selector(params BTNode[] nodes)
    {
        this.nodes = nodes;
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Select Nodes
        foreach (BTNode n in nodes)
        {
            TaskStatus status = n.Run();
            switch (status)
            {
                case TaskStatus.Completed: return TaskStatus.Completed;
                case TaskStatus.Failed: continue;
                case TaskStatus.Running: return TaskStatus.Running;
            }
        }
        return TaskStatus.Completed;
    }

}//CLASS
