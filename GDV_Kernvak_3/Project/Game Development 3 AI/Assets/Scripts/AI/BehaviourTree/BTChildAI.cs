using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTChildAI : MonoBehaviour
{
    //BehaviourTree Nodes
    public BlackBoard BB;
    private BTNode root;
    private BTNode RetreatBranch;
    private BTNode HealOrCry;
    private BTNode Heal;
    private BTNode AttackBranch;
    private BTNode Combine;
    private BTNode AttackOrCombine;

    //MotherAI Stats
    public float MaxHealth = 100.0f;
    public float CurrentHealth;
    public float DangerHP = 30.0f;
    public float DestinationRadius = 1.0f;
    public float MovementSpeed = 5.0f;
    public float CD = 10.0f;
    public Slider SS;
    public GameObject CloseAttack;
    public GameObject RangedAttack;

    //Audio
    private AudioSource AS;
    public AudioClip CrySound;
    public AudioClip HealMeSound;
    public AudioClip HeySound;
    public AudioClip Hit;

    private void Awake()
    {
        //Init Pathfinding
        Pathfinding PF = gameObject.GetComponent<Pathfinding>();
        if (PF == null)
        {
            PF = gameObject.AddComponent<Pathfinding>();
        }
        //Init Audio
        AS = GetComponent<AudioSource>();
        //Set HP
        CurrentHealth = MaxHealth;
        //Set Begin Values
        RangedAttack.SetActive(false);
        CloseAttack.SetActive(false);       
        //Create BlackBoard
        BB = new BlackBoard(
            new KeyValuePair<string, object>("MaxHealth", MaxHealth),
            new KeyValuePair<string, object>("CurrentHealth", CurrentHealth),
            new KeyValuePair<string, object>("DangerHP", DangerHP),
            new KeyValuePair<string, object>("NearPlayer", false),
            new KeyValuePair<string, object>("NearMother", false),
            new KeyValuePair<string, object>("NearChild", false),
            new KeyValuePair<string, object>("HasCooldown", false),
            new KeyValuePair<string, object>("InHealing", false),
            new KeyValuePair<string, object>("HasRangedAttack", false),
            new KeyValuePair<string, object>("CD", CD),
            new KeyValuePair<string, object>("CoolDown", 0.0f),
            new KeyValuePair<string, object>("RangedAttack", RangedAttack),
            new KeyValuePair<string, object>("CloseAttack", CloseAttack),
            new KeyValuePair<string, object>("IdentityMotherAI", GameObject.FindGameObjectWithTag("MotherAI")),
            new KeyValuePair<string, object>("IdentityPlayer", GameObject.FindGameObjectWithTag("Player")),
            new KeyValuePair<string, object>("FollowPlayer", false),
            new KeyValuePair<string, object>("DestinationRadius", DestinationRadius),
            new KeyValuePair<string, object>("PF", PF),
            new KeyValuePair<string, object>("RandomPos", PF.RandomDestination()),
            new KeyValuePair<string, object>("MovementSpeed", MovementSpeed),
            new KeyValuePair<string, object>("AS", AS),
            new KeyValuePair<string, object>("CrySound", CrySound),
            new KeyValuePair<string, object>("HealMeSound", HealMeSound),
            new KeyValuePair<string, object>("HeySound", HeySound),
            new KeyValuePair<string, object>("CAI_Healing", new ChildAI()),
            new KeyValuePair<string, object>("HasHealRequest", false),
            new KeyValuePair<string, object>("IsEnergizingWomb", false),
            new KeyValuePair<string, object>("Self", gameObject));
        Heal = new Sequence(new FindMother(BB), new SendHealRequest(BB, this));
        HealOrCry = new Selector(Heal, new Cry(BB));
        RetreatBranch = new Sequence(new CheckHP(BB), HealOrCry);
        Combine = new Sequence(new FindChild(BB), new CombinePowers(BB));
        AttackOrCombine = new Selector(Combine, new AttackPlayer(BB));
        AttackBranch = new Sequence(new FindPlayer(BB), AttackOrCombine);
        root = new Selector(RetreatBranch, AttackBranch);
    }

    //Update every frame
    public void Update()
    {
        //Behaviour Tree
        root.Run();

        //Cooldown Mechanism
        if(BB.GetValue<float>("CoolDown") > 0)
        {           
            BB.SetValue("HasCooldown", true);
            BB.SetValue("HasRangedAttack", true);

            float CoolDown = BB.GetValue<float>("CoolDown");
            CoolDown -= 1 * Time.deltaTime;
            BB.SetValue("CoolDown", CoolDown);
            if (CoolDown <= 0)
            {
                BB.SetValue("HasCooldown", false);
                BB.SetValue("HasRangedAttack", false);
                CoolDown = 0;
                BB.SetValue("CoolDown", CoolDown);
            }
        }
    }

    //AI Die State
    private void Die()
    {
        print("ChildAI: " + gameObject.name + " Dies");
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

    //Identify if the other ChildAI is potentional
    private void IdentifyChild(GameObject Target)
    {
        if (!BB.GetValue<bool>("HasCooldown"))
        {
            BTChildAI CAI = Target.GetComponent<BTChildAI>();

            if (CAI != null)
            {
                if (!CAI.BB.GetValue<bool>("HasCooldown")) { BB.SetValue("NearChild", true); }
            }
        }
    }

    //Sensor for entering objects
    private void OnTriggerEnter(Collider Target)
    {
        //Identify entered object
        switch (Target.gameObject.tag)
        {
            case "Player": BB.SetValue("NearPlayer", true); BB.SetValue("FollowPlayer", true); AS.PlayOneShot(HeySound); break;
            case "MotherAI": BB.SetValue("NearMother", true); break;
            case "ChildAI": IdentifyChild(Target.gameObject); break;
        }
    }

    //Sensor for exiting object
    private void OnTriggerExit(Collider Target)
    {
        //Identify exited object
        switch (Target.gameObject.tag)
        {
            case "Player": BB.SetValue("NearPlayer", false); break;
            case "MotherAI": BB.SetValue("NearMother", false); break;
            case "ChildAI": BB.SetValue("NearChild", true); break;
        }
    }

}//CLASS
