using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindChild : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private bool NearChild;
    private bool HasCooldown;

    //Constructor
    public FindChild(BlackBoard BB)
    {
        this.BB = BB;
        NearChild = BB.GetValue<bool>("NearChild");
        HasCooldown = BB.GetValue<bool>("HasCooldown");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        NearChild = BB.GetValue<bool>("NearChild");
        HasCooldown = BB.GetValue<bool>("HasCooldown");
        //Set TaskState
        if (NearChild && !HasCooldown)
        {
            return TaskStatus.Completed;
        }
        else
        {
            return TaskStatus.Failed;            
        }
    }
}//CLASS
