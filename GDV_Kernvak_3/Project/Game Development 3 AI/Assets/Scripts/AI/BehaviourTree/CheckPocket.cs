using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPocket : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private float MaxEnergyPocket;
    private float CurrentEnergyPocket;

    //Constructor
    public CheckPocket(BlackBoard BB)
    {
        this.BB = BB;
        MaxEnergyPocket = BB.GetValue<float>("MaxEnergyPocket");
        CurrentEnergyPocket = BB.GetValue<float>("CurrentEnergyPocket");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        CurrentEnergyPocket = BB.GetValue<float>("CurrentEnergyPocket");
        //Check Pocket
        if (CurrentEnergyPocket >= MaxEnergyPocket)
        {
            return TaskStatus.Completed;
        }
        else if (BB.GetValue<bool>("IsEnergizingWomb"))
        {
            return TaskStatus.Completed;
        }
        else
        {
            return TaskStatus.Failed;
        }
    }

}//CLASS
