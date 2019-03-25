using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private bool FollowPlayer;
    private GameObject CloseAttack;
    private GameObject RangedAttack;
    private bool HasRangedAttack;

    //Constructor
    public AttackPlayer(BlackBoard BB)
    {
        this.BB = BB;
        FollowPlayer = BB.GetValue<bool>("FollowPlayer");
        CloseAttack = BB.GetValue<GameObject>("CloseAttack");
        RangedAttack = BB.GetValue<GameObject>("RangedAttack");
        HasRangedAttack = BB.GetValue<bool>("HasRangedAttack");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        HasRangedAttack = BB.GetValue<bool>("HasRangedAttack");
        
        //Set ChildAI Attacking State
        if (HasRangedAttack)
        {
            RangedAttack.SetActive(true);
            CloseAttack.SetActive(true);
        }
        else
        {
            RangedAttack.SetActive(false);
            CloseAttack.SetActive(true);
        }

        return TaskStatus.Running;
    }
}
