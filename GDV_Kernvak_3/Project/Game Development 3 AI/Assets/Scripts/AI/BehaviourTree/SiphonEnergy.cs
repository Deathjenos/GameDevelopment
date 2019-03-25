using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiphonEnergy : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private GameObject ESObject;
    private EnergySource ES;
    private float TransitionEnergyPerSecond;
    private float CurrentEnergyPocket;
    private float MaxEnergyPocket;

    //Constructor
    public SiphonEnergy(BlackBoard BB)
    {
        this.BB = BB;
        ESObject = BB.GetValue<GameObject>("BestSource");
        ES = ESObject.GetComponent<EnergySource>();
        TransitionEnergyPerSecond = BB.GetValue<float>("TransitionEnergyPerSecond");
        CurrentEnergyPocket = BB.GetValue<float>("CurrentEnergyPocket");
        MaxEnergyPocket = BB.GetValue<float>("MaxEnergyPocket");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Update Variables
        CurrentEnergyPocket = BB.GetValue<float>("CurrentEnergyPocket");
        //Decrease Energy from Energy Source
        float DrawnEnergy = ES.SiphonEnergy(TransitionEnergyPerSecond * Time.deltaTime);
        //Increase Energy from Pocket
        CurrentEnergyPocket += DrawnEnergy;
        BB.SetValue("CurrentEnergyPocket", CurrentEnergyPocket);      

        //Stop Siphon when
        if (DrawnEnergy == 0)
        {
            ES.StopSiphon();
            return TaskStatus.Completed;
        }
        if (CurrentEnergyPocket >= MaxEnergyPocket)
        {
            CurrentEnergyPocket = MaxEnergyPocket;
            BB.SetValue("CurrentEnergyPocket", CurrentEnergyPocket);
            ES.StopSiphon();
            return TaskStatus.Completed;
        }

        return TaskStatus.Running;
    }

}//CLASS
