using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealChild : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private BTChildAI CAI_Healing;
    private float HealPerSecond;

    //Constructor
    public HealChild(BlackBoard BB)
    {
        this.BB = BB;
        CAI_Healing = BB.GetValue<BTChildAI>("CAI_Healing");
        HealPerSecond = BB.GetValue<float>("HealPerSecond");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        CAI_Healing = BB.GetValue<BTChildAI>("CAI_Healing");
        //Heal Child
        if (CAI_Healing != null)
        {
            if (CAI_Healing.CurrentHealth == CAI_Healing.MaxHealth)
            {
                CAI_Healing.BB.SetValue("InHealing", false);
                return TaskStatus.Completed;
            }
            else
            {
                CAI_Healing.BB.SetValue("InHealing", true);
                //Amount of health to add
                float AmountAddHP = HealPerSecond * Time.deltaTime;
                //Heal ChildAI
                CAI_Healing.TranslateHP(AmountAddHP);

                return TaskStatus.Running;
            }           
        }
        else
        {
            return TaskStatus.Failed;
        }
        
    }

}//CLASS
