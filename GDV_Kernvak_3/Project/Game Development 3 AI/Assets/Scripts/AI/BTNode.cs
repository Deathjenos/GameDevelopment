using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskStatus
{
    Running,
    Completed,
    Failed
}

public abstract class BTNode {

    public abstract TaskStatus Run();
}

//Composite Nodes (Sequence, Selector)
//Task Nodes (Atomair)



