using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendHealRequest : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private GameObject IdentityMotherAI;
    private BTMotherAI MAI;
    private BTChildAI CAI;

    //Constructor
    public SendHealRequest(BlackBoard BB, BTChildAI CAI)
    {
        this.BB = BB;
        IdentityMotherAI = BB.GetValue<GameObject>("IdentityMotherAI");
        MAI = IdentityMotherAI.GetComponent<BTMotherAI>();
        this.CAI = CAI;
    }

    public override TaskStatus Run()
    {
        //Send heal request
        MAI.HealRequest(CAI);

        return TaskStatus.Completed;
    }
}
