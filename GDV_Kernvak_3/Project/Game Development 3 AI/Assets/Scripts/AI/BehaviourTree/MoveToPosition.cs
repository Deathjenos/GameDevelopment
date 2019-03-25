using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : BTNode
{
    //Node Stats
    private CheckAtLocation CAL;
    private Pathfinding PF;
    private List<Node> Path;
    private GameObject SelfObject;
    private Vector3 TargetPosition;
    private float MovementSpeed;

    //Constructor
    public MoveToPosition(BlackBoard BB, Vector3 TargetPosition)
    {
        PF = BB.GetValue<Pathfinding>("PF");
        SelfObject = BB.GetValue<GameObject>("Self");
        this.TargetPosition = TargetPosition;
        MovementSpeed = BB.GetValue<float>("MovementSpeed");
        CAL = new CheckAtLocation(BB, TargetPosition);
    }

    //Run Method
    public override TaskStatus Run()
    {
        //If at Location
        if (CAL.Run() == TaskStatus.Completed)
        {
            return TaskStatus.Completed;
        }
        //If not at Location
        else
        {
            //Find Path
            PF.FindPath(SelfObject.transform.position, TargetPosition);
            Path = PF.FinalPath;
            //Move on Path
            if (Path != null)
            {
                //Move and Look on Path
                Vector3 TargetPos = new Vector3(Path[0].Position.x, SelfObject.transform.position.y, Path[0].Position.z);
                SelfObject.transform.position = Vector3.MoveTowards(SelfObject.transform.position, TargetPos, MovementSpeed * Time.deltaTime);
                SelfObject.transform.LookAt(TargetPos);
                //At Final Location?
                if (CAL.Run() == TaskStatus.Completed)
                {
                    return TaskStatus.Completed;
                }
                else
                {
                    return TaskStatus.Running;
                }
            }
            return TaskStatus.Running;
        }
    }

}//CLASS
