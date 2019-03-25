using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAtLocation : BTNode
{
    //Node Stats
    BlackBoard BB;
    private GameObject SelfObject;
    private GameObject TargetObject;
    private Vector3 TargetPosition;
    private float DestinationRadius;

    //Constructors
    public CheckAtLocation(BlackBoard BB, GameObject TargetObject)
    {
        this.BB = BB;
        DestinationRadius = BB.GetValue<float>("DestinationRadius");
        this.TargetObject = TargetObject;
        TargetPosition = TargetObject.transform.position;
        SelfObject = BB.GetValue<GameObject>("Self");
    }
    public CheckAtLocation(BlackBoard BB, Vector3 TargetPosition)
    {
        this.BB = BB;
        DestinationRadius = BB.GetValue<float>("DestinationRadius");
        this.TargetPosition = TargetPosition;
        SelfObject = BB.GetValue<GameObject>("Self");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Check if at location
        if (AtLocation())
        {
            return TaskStatus.Completed;
        }
        else
        {
            return TaskStatus.Failed;
        }
    }

    //Check At Location
    private bool AtLocation()
    {
        float DistanceToLocation = Vector3.Distance(SelfObject.transform.position, TargetPosition);

        if (DistanceToLocation <= DestinationRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}//CLASS
