using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WombScript : MonoBehaviour {

    //Womb Stats
    public float NeededBirthEnergy = 100.0f;
    public float CurrentEnergy = 0.0f;
    public bool MayGiveBirth = false;
    public GameObject ChildAI;
    public Vector3 BirthSpawnPosition = new Vector3(0, 0, 0);
	

    //Increase Energy of Womb
    public void IncreaseWombEnergy(float amount)
    {
        CurrentEnergy += amount;
        CheckBirth();
    }

    //Check Birth
    private void CheckBirth()
    {
        if (CurrentEnergy >= NeededBirthEnergy && !MayGiveBirth)
        {
            MayGiveBirth = true;
            CurrentEnergy -= NeededBirthEnergy;
        }
    }

    //Give birth, Create child
    public void GiveBirth()
    {
        if (MayGiveBirth && ChildAI != null)
        {
            Instantiate(ChildAI, BirthSpawnPosition, Quaternion.identity);
            MayGiveBirth = false;
            CheckBirth();
        }
    }

}//CLASS
