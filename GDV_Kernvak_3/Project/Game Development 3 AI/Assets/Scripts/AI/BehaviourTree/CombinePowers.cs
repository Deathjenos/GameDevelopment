using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinePowers : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private float CD;

    //Constructor
    public CombinePowers(BlackBoard BB)
    {
        this.BB = BB;
        CD = BB.GetValue<float>("CD");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Set Special Attack State
        BB.SetValue("HasCooldown", true);
        BB.SetValue("HasRangedAttack", true);
        BB.SetValue("CoolDown", CD);

        return TaskStatus.Completed;
    }
}
