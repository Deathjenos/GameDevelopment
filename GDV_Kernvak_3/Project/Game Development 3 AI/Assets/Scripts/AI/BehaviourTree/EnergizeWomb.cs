using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergizeWomb : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private GameObject Womb;
    private WombScript WS;
    private float CurrentEnergyPocket;
    private float TransitionEnergyPerSecond;

    //Constructor
    public EnergizeWomb(BlackBoard BB)
    {
        this.BB = BB;
        Womb = BB.GetValue<GameObject>("Womb");
        WS = Womb.GetComponent<WombScript>();
        CurrentEnergyPocket = BB.GetValue<float>("CurrentEnergyPocket");
        TransitionEnergyPerSecond = BB.GetValue<float>("TransitionEnergyPerSecond");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        CurrentEnergyPocket = BB.GetValue<float>("CurrentEnergyPocket");
        //Energize Womb
        if (CurrentEnergyPocket >= 0)
        {
            BB.SetValue("IsEnergizingWomb", true);
            float TransmissionAmount = TransitionEnergyPerSecond * Time.deltaTime;
            //Decrease EnergyPocket
            CurrentEnergyPocket -= TransmissionAmount;
            BB.SetValue("CurrentEnergyPocket", CurrentEnergyPocket);

            //When done Energizing
            if (CurrentEnergyPocket <= 0)
            {
                TransmissionAmount += CurrentEnergyPocket;
                CurrentEnergyPocket = 0;
                BB.SetValue("CurrentEnergyPocket", CurrentEnergyPocket);
            }

            //Increase Womb Energy
            WS.IncreaseWombEnergy(TransmissionAmount);
        }
        if(CurrentEnergyPocket == 0)
        {
            BB.SetValue("IsEnergizingWomb", false);
            return TaskStatus.Completed;
        }

        return TaskStatus.Running;
    }

}//CLASS
