using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBirth : BTNode
{
    //Node Stats
    BlackBoard BB;
    private GameObject Womb;
    private WombScript WS;

    //Constructors
    public CheckBirth(BlackBoard BB)
    {
        this.BB = BB;
        Womb = BB.GetValue<GameObject>("Womb");
        WS = Womb.GetComponent<WombScript>();
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Check Birth Posibility
        if (WS.MayGiveBirth)
        {
            return TaskStatus.Completed;
        }
        else
        {
            return TaskStatus.Failed;
        }
    }

}//CLASS
