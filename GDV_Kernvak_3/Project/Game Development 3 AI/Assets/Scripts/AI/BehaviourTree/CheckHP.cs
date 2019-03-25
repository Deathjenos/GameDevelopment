using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHP : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private float MaxHealth;
    private float CurrentHealth;
    private float DangerHP;
    private bool NearMother;
    private bool InHealing;
    private GameObject CloseAttack;
    private GameObject RangedAttack;

    //Constructor
    public CheckHP(BlackBoard BB)
    {
        this.BB = BB;
        MaxHealth = BB.GetValue<float>("MaxHealth");
        CurrentHealth = BB.GetValue<float>("CurrentHealth");
        DangerHP = BB.GetValue<float>("DangerHP");
        NearMother = BB.GetValue<bool>("NearMother");
        InHealing = BB.GetValue<bool>("InHealing");
        CloseAttack = BB.GetValue<GameObject>("CloseAttack");
        RangedAttack = BB.GetValue<GameObject>("RangedAttack");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        CurrentHealth = BB.GetValue<float>("CurrentHealth");
        NearMother = BB.GetValue<bool>("NearMother");
        InHealing = BB.GetValue<bool>("InHealing");

        //Check for dangerous HP levels
        if (CurrentHealth <= DangerHP)
        {
            BB.SetValue("FollowPlayer", false);
            RangedAttack.SetActive(false);
            CloseAttack.SetActive(false);
            return TaskStatus.Completed;
        }
        else if (NearMother && InHealing)
        {
            BB.SetValue("FollowPlayer", false);
            RangedAttack.SetActive(false);
            CloseAttack.SetActive(false);
            return TaskStatus.Completed;
        }
        else
        {
            BB.SetValue("InHealing", false);
            return TaskStatus.Failed;
        }
    }
}
