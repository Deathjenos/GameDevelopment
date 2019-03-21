using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySource : MonoBehaviour {

    //EnergySource Stats
    public float MaxEnergy = 100.0f;
    public float GenerationPerSecond = 1.0f;
    public float CurrentEnergy = 0.0f;

    //EnergySource Variables
    private bool MayEnergize = true;


	//Update every frame
	private void Update () {
        //Energizing 
        if (MayEnergize)
        {
            //Add energy
            CurrentEnergy = CurrentEnergy + (GenerationPerSecond * Time.deltaTime);

            //Check Energy Levels
            if(CurrentEnergy >= MaxEnergy)
            {
                CurrentEnergy = MaxEnergy;
                MayEnergize = false;
            }
        }
	}

    //Announcing Start Siphon
    public void StartSiphon()
    {
        //Stop Energy Renegation
        MayEnergize = false;
    }

    //Siphon Energy from EnergySource
    public float SiphonEnergy(float Amount)
    {
        //Calculate Energy
        float TakenEnergy = Amount;
        CurrentEnergy -= Amount;
        //Recalculate Energy if below 0
        if(CurrentEnergy <= 0)
        {
            TakenEnergy += CurrentEnergy;
            CurrentEnergy = 0;
        }

        return TakenEnergy;
    }

    //Announcing Stop Siphon
    public void StopSiphon()
    {
        //Start Energy Renegation
        MayEnergize = true;
    }

}//CLASS
