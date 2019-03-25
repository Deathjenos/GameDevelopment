using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBestEnergySource : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private List<EnergySource> ESList;
    private GameObject[] ESObjectList;

    //Constructor
    public FindBestEnergySource(BlackBoard BB)
    {
        this.BB = BB;
        ESList = BB.GetValue<List<EnergySource>>("ESList");
        ESObjectList = BB.GetValue<GameObject[]>("ESObjectList");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Chose best EnergySource
        int ChosenES = 0;
        float Energy = 0.0f;
        for (int i = 0; i < ESList.Count; i++)
        {
            if (ESList[i].CurrentEnergy > Energy)
            {
                ChosenES = i;
                Energy = ESList[i].CurrentEnergy;
            }
        }
        GameObject BestEnergySource = ESObjectList[ChosenES];
        BB.SetValue("BestSource", BestEnergySource);

        return TaskStatus.Completed;
    }

}//CLASS
