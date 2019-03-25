using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHealRequest : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private bool HasHealRequest;

    //Constructor
    public CheckHealRequest(BlackBoard BB)
    {
        this.BB = BB;
        HasHealRequest = BB.GetValue<bool>("HasHealRequest");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        HasHealRequest = BB.GetValue<bool>("HasHealRequest");
        //Set TaskState
        if (HasHealRequest)
        {
            return TaskStatus.Completed;
        }
        else
        {
            return TaskStatus.Failed;
        }   
    }

}//CLASS
