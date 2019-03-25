using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayer : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private bool NearPlayer;
    private Vector3 RandomPos;
    private Pathfinding PF;
    private GameObject IdentityPlayer;

    //Constructor
    public FindPlayer(BlackBoard BB)
    {
        this.BB = BB;
        NearPlayer = BB.GetValue<bool>("NearPlayer");
        RandomPos = BB.GetValue<Vector3>("RandomPos");
        PF = BB.GetValue<Pathfinding>("PF");
        IdentityPlayer = BB.GetValue<GameObject>("IdentityPlayer");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        NearPlayer = BB.GetValue<bool>("NearPlayer");
        RandomPos = BB.GetValue<Vector3>("RandomPos");
        //Set TaskState
        if (new CheckAtLocation(BB, IdentityPlayer).Run() == TaskStatus.Completed)
        {
            Debug.LogWarning("ChildAI: Found Player");
            return TaskStatus.Completed;
        }
        if (BB.GetValue<bool>("FollowPlayer"))
        {
            new MoveToObject(BB, IdentityPlayer).Run();
            return TaskStatus.Running;
        }
        else if (new CheckAtLocation(BB, RandomPos).Run() == TaskStatus.Failed)
        {
            new MoveToPosition(BB, RandomPos).Run();
            return TaskStatus.Running;
        }
        else
        {
            BB.SetValue("RandomPos", PF.RandomDestination());
            return TaskStatus.Running;
        }
    }

}//CLASS
