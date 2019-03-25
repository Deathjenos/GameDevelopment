using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherAI : AIBasics {

    //Public Variables
    public float TransitionEnergyPerSecond = 1.0f;
    public float HealPerSecond = 1.0f;
    public float BirthTime = 1.0f;
    public float SelfHealPerSecond = 1.0f;
    public AudioClip ScreamSound;

    //MotherAI Stats
    public float MaxEnergyPocket = 50.0f;
    public float CurrentEnergyPocket = 0.0f;

    //MotherAI Knowledge Library
    private GameObject[] ESObjectList;
    private List<EnergySource> ESList = new List<EnergySource>();
    private GameObject Womb;
    private WombScript WS;
    private ChildAI CAI_Healing;
    public GameStateManager GSM;

    //BehaviourTree Variables
    private int CurrentState = 0;
    private int ChosenES;

    //Behaviour States
    //0 = Idle
    //1 = Moving
    //2 = Giving Birth
    //3 = Healing Child
    //4 = Energizing Womb
    //5 = Energizing Pocket

    //Initialization
    protected override void Awake () {
        //Execute Parent Awake
        base.Awake();

        //Find all Object
        Womb = GameObject.FindGameObjectWithTag("Womb");
        WS = Womb.GetComponent<WombScript>();
        ESObjectList = GameObject.FindGameObjectsWithTag("EnergySource");
        foreach (GameObject k in ESObjectList)
        {
            EnergySource ES = k.GetComponent<EnergySource>();
            if (ES != null)
            {
                ESList.Add(ES);
            }
        }
        //Init AudioSource
        AS = GetComponent<AudioSource>();
    }
	
	//Update every frame
	protected override void Update () {
        //Execute Parent Update
        base.Update();

        //BehaviourTree 
        BT();
         
        //States Execution
        if (CurrentState == 3) { HealChild(); }
        if (CurrentState == 4) { EnergizeWomb(); }
        if (CurrentState == 5) { EnergizePocket(); }

        //Heal Regenation
        if(GSM.AmountOfChildAI > 0 && CurrentHealth < MaxHealth)
        {
            TranslateHP(SelfHealPerSecond * Time.deltaTime);
        }
    }

    //AI Die State
    protected override void Die()
    {
        print("MotherAI: " + gameObject.name + " Dies");
        //Delete this GameObject
        Destroy(gameObject);
    }

    //BehaviourTree
    private void BT()
    {
        //First Check Birth Posibility
        if (WS.MayGiveBirth)
        {
            //Give Birth
            WS.GiveBirth();
            StartCoroutine(GiveBirth());
        }
        //Second Check Energy Pocket
        else if(CurrentEnergyPocket == MaxEnergyPocket && CurrentState != 3 && CurrentState != 2)
        {
            //Move to Womb if not there
            if (!CheckAtLocation(Womb))
            {
                CurrentState = 1;
                MoveToObject(Womb);
            }
            //Donate Energy
            if (CheckAtLocation(Womb))
            {
                CurrentState = 4;
            }
        }
        //Third Energize Energy Pocket
        else if (CurrentEnergyPocket <= MaxEnergyPocket && CurrentState != 2 && CurrentState != 3 && CurrentState != 4 && CurrentState != 5)
        {
            //Find EnergySource with the most Energy
            ChosenES = 0;
            float Energy = 0.0f;
            for(int i = 0; i < ESObjectList.Length; i++)
            {
                if (ESList[i].CurrentEnergy > Energy)
                {
                    ChosenES = i;
                    Energy = ESList[i].CurrentEnergy;
                }
            }
            //Go To EnergySource
            if (!CheckAtLocation(ESObjectList[ChosenES]))
            {
                CurrentState = 1;
                MoveToObject(ESObjectList[ChosenES]);
            }
            //Go To EnergySource
            if (CheckAtLocation(ESObjectList[ChosenES]))
            {
                CurrentState = 5;
                ESList[ChosenES].StartSiphon();
            }
        }
    }

    //GiveBirth State
    private IEnumerator GiveBirth()
    {
        //Play Screaming Sound
        AS.PlayOneShot(ScreamSound);
        //Stand still
        AtFocusLocation = true;
        CurrentState = 2;
        //Wait 2 seconds
        yield return new WaitForSeconds(2.0f);
        //Reset State
        CurrentState = 0;
    }

    //Request of ChildAI to Heal it
    public bool RequestHeal(ChildAI CAI)
    {
        //Set ChildAI to heal
        CAI_Healing = CAI;

        //Accept Heal Request if not giving birth
        if (CurrentState != 2)
        {
            CurrentState = 3;

            return true;
        }
        else
        {
            return false;
        }

    }

    //HealChild State
    private void HealChild()
    {
        //if the ChildAI is still there
        if (CAI_Healing != null)
        {
            //Amount of health to add
            float AmountAddHP = HealPerSecond * Time.deltaTime;
            //Heal ChildAI
            CAI_Healing.TranslateHP(AmountAddHP);

            //Continue when done healing
            if (CAI_Healing.CurrentHealth == CAI_Healing.MaxHealth)
            {
                CurrentState = 0;
            }
        }
        //Else Reset State
        else
        {
            CurrentState = 0;
        }
        
    }

    //EnergizeWomb State
    private void EnergizeWomb()
    {
        float TransmissionAmount = TransitionEnergyPerSecond * Time.deltaTime;
        //Decrease EnergyPocket
        CurrentEnergyPocket -= TransmissionAmount;

        //When done Energizing
        if(CurrentEnergyPocket <= 0)
        {
            TransmissionAmount += CurrentEnergyPocket;
            CurrentEnergyPocket = 0;
            CurrentState = 0;
        }

        //Increase Womb Energy
        WS.IncreaseWombEnergy(TransmissionAmount);
    }

    //EnergizeWomb State
    private void EnergizePocket()
    {
        //Decrease Energy from Energy Source
        float DrawnEnergy = ESList[ChosenES].SiphonEnergy(TransitionEnergyPerSecond * Time.deltaTime);
        //Increase Energy from Pocket
        CurrentEnergyPocket += DrawnEnergy;

        //Stop Siphon when
        if(DrawnEnergy == 0)
        {
            ESList[ChosenES].StopSiphon();
            CurrentState = 0;
        }
        if(CurrentEnergyPocket >= MaxEnergyPocket)
        {
            CurrentEnergyPocket = MaxEnergyPocket;
            ESList[ChosenES].StopSiphon();
            CurrentState = 0;
        }

    }

}//CLASS
