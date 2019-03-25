using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTMotherAI : MonoBehaviour {
    //BehaviourTree Nodes
    private BlackBoard BB;
    private BTNode root;
    private BTNode BirthBranch;
    private BTNode HealChildBranch;
    private BTNode EnergizeWombBranch;
    private BTNode EnergizeEnergySourceBranch;

    //MotherAI Stats
    public float MaxHealth = 100.0f;
    public float CurrentHealth;
    public float DestinationRadius = 1.0f;
    public float MovementSpeed = 5.0f;
    public float HealPerSecond = 1.0f;
    public float MaxEnergyPocket = 100.0f;
    public float CurrentEnergyPocket = 0.0f;
    public float TransitionEnergyPerSecond = 1.0f;
    public float SelfHealPerSecond = 1.0f;
    public Slider SS;

    private List<EnergySource> ESList = new List<EnergySource>();
    private GameObject[] ESObjectList;
    public GameStateManager GSM;

    //Audio
    private AudioSource AS;
    public AudioClip ScreamSound;
    public AudioClip Hit;

    //Initializiation
    private void Awake()
    {
        //Init Pathfinding
        Pathfinding PF = gameObject.GetComponent<Pathfinding>();
        if (PF == null)
        {
            PF = gameObject.AddComponent<Pathfinding>();
        }
        //Find EnergySources
        ESObjectList = GameObject.FindGameObjectsWithTag("EnergySource");
        foreach (GameObject k in ESObjectList)
        {
            EnergySource ES = k.GetComponent<EnergySource>();
            if (ES != null)
            {
                ESList.Add(ES);
            }
        }
        //Init Audio
        AS = GetComponent<AudioSource>();
        
        //Create BlackBoard
        BB = new BlackBoard(
            new KeyValuePair<string, object>("MaxHealth", MaxHealth),
            new KeyValuePair<string, object>("CurrentHealth", CurrentHealth),
            new KeyValuePair<string, object>("Womb", GameObject.FindGameObjectWithTag("Womb")),
            new KeyValuePair<string, object>("DestinationRadius", DestinationRadius),
            new KeyValuePair<string, object>("PF", PF),
            new KeyValuePair<string, object>("MovementSpeed", MovementSpeed),
            new KeyValuePair<string, object>("ESList", ESList),
            new KeyValuePair<string, object>("ESObjectList", ESObjectList),
            new KeyValuePair<string, object>("AS", AS),
            new KeyValuePair<string, object>("ScreamSound", ScreamSound),
            new KeyValuePair<string, object>("BestSource", ESObjectList[0]),
            new KeyValuePair<string, object>("CAI_Healing", new BTChildAI()),
            new KeyValuePair<string, object>("HasHealRequest", false),
            new KeyValuePair<string, object>("IsEnergizingWomb", false),
            new KeyValuePair<string, object>("HealPerSecond", HealPerSecond),
            new KeyValuePair<string, object>("MaxEnergyPocket", MaxEnergyPocket),
            new KeyValuePair<string, object>("CurrentEnergyPocket", CurrentEnergyPocket),
            new KeyValuePair<string, object>("TransitionEnergyPerSecond", TransitionEnergyPerSecond),
            new KeyValuePair<string, object>("Self", gameObject));
        BirthBranch = new Sequence(new CheckBirth(BB), new MoveToObject(BB, BB.GetValue<GameObject>("Womb")), new GiveBirth(BB));
        HealChildBranch = new Sequence(new CheckHealRequest(BB), new HealChild(BB));
        EnergizeWombBranch = new Sequence(new CheckPocket(BB), new MoveToObject(BB, BB.GetValue<GameObject>("Womb")), new EnergizeWomb(BB));
        EnergizeEnergySourceBranch = new Sequence(new FindBestEnergySource(BB), new MoveToObject(BB, BB.GetValue<GameObject>("BestSource")), new SiphonEnergy(BB));
        root = new Selector(BirthBranch, HealChildBranch, EnergizeWombBranch, EnergizeEnergySourceBranch);
    }

    //Updates every frame
    public void Update()
    {
        //Behaviour Tree
        root.Run();

        //Heal Regenation
        if (GSM.AmountOfChildAI > 0 && CurrentHealth < MaxHealth)
        {
            TranslateHP(SelfHealPerSecond * Time.deltaTime);
        }
    }

    //Heal Request for ChildAI's
    public void HealRequest(BTChildAI CAI)
    {
        BB.SetValue("HasHealRequest", true);
        BB.SetValue("CAI_Healing", CAI);
    }

    //AI Die State
    private void Die()
    {
        print("MotherAI: " + gameObject.name + " Dies");
        //Delete this GameObject
        Destroy(gameObject);
    }

    //Increase or Decrease CurrentHealth
    public void TranslateHP(float amount)
    {
        //Calculate new Current Health
        CurrentHealth = BB.GetValue<float>("CurrentHealth");
        CurrentHealth += amount;
        BB.SetValue("CurrentHealth", CurrentHealth);

        //Die when Current Health is empty
        if (CurrentHealth <= 0)
        {
            Die();
        }
        //Check Full HP
        if (CurrentHealth >= BB.GetValue<float>("MaxHealth"))
        {
            CurrentHealth = BB.GetValue<float>("MaxHealth");
            BB.SetValue("CurrentHealth", CurrentHealth);
        }
        //Play Hitting Sound when Decreasing Health
        if (amount < 0)
        {
            AS.PlayOneShot(Hit);
        }

        //Set Health Visuals to UI
        SetSliderVisuals();
    }

    //Set Health Visuals to UI
    private void SetSliderVisuals()
    {
        float procent = BB.GetValue<float>("CurrentHealth") / BB.GetValue<float>("MaxHealth");
        SS.value = procent;
    }

}//CLASS
