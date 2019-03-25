using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMother : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private bool NearMother;
    private Vector3 RandomPos;
    private Pathfinding PF;

    //Constructor
    public FindMother(BlackBoard BB)
    {
        this.BB = BB;
        NearMother = BB.GetValue<bool>("NearMother");
        RandomPos = BB.GetValue<Vector3>("RandomPos");
        PF = BB.GetValue<Pathfinding>("PF");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        NearMother = BB.GetValue<bool>("NearMother");
        RandomPos = BB.GetValue<Vector3>("RandomPos");
        //Set TaskStatus
        if (NearMother)
        {
            return TaskStatus.Completed;
        }
        else if (new CheckAtLocation(BB, RandomPos).Run() == TaskStatus.Failed)
        {
            new MoveToPosition(BB, RandomPos).Run();
            return TaskStatus.Running;
        }
        else
        {
            BB.SetValue("RandomPos", PF.RandomDestination());
            return TaskStatus.Failed;
        }
    }

}//CLASS
