using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToObject : BTNode
{
    //Node Variables
    private CheckAtLocation CAL;
    private Pathfinding PF;
    private List<Node> Path;
    private GameObject SelfObject;
    private GameObject TargetObject;
    private float MovementSpeed;

    //Constructor
	public MoveToObject(BlackBoard BB, GameObject TargetObject)
    {
        PF = BB.GetValue<Pathfinding>("PF");
        SelfObject = BB.GetValue<GameObject>("Self");
        this.TargetObject = TargetObject;
        MovementSpeed = BB.GetValue<float>("MovementSpeed");
        CAL = new CheckAtLocation(BB, TargetObject);
    }

    //Run Method
    public override TaskStatus Run()
    {
        //If at Location
        if(CAL.Run() == TaskStatus.Completed)
        {
            return TaskStatus.Completed;
        }
        //If not at Location
        else
        {
            //Find Path
            PF.FindPath(SelfObject.transform.position, TargetObject.transform.position);
            Path = PF.FinalPath;
            //Move on Path
            if (Path != null)
            {
                //Move and Look on Path
                Vector3 TargetPosition = new Vector3(Path[0].Position.x, SelfObject.transform.position.y, Path[0].Position.z);
                SelfObject.transform.position = Vector3.MoveTowards(SelfObject.transform.position, TargetPosition, MovementSpeed * Time.deltaTime);
                SelfObject.transform.LookAt(TargetPosition);
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
