using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAI : AIBasics {
    //Public variables
    public float CombineCD = 10.0f;         //Combine Power Cooldown Time
    public float DangerHP = 30.0f;          //At what HP to retreat
    public float RangedAttackTime = 5.0f;
    public AudioClip CrySound;
    public AudioClip HealMeSound;
    public AudioClip HeySound;

    //ChildAI Stats
    public float Cooldown = 0.0f;
    public bool HasCooldown = false;
    public GameObject CloseAttack;
    public GameObject RangedAttack;
    private bool HasRangedAttack = false;

    //Child AI Knowledge Library
    private GameObject IdentityMotherAI;
    private GameObject IdentityPlayer;
    private bool NearMother = false;
    private bool NearPlayer = false;
    private bool NearChild = false;
    private Vector3 RandomPos = new Vector3(0, 0, 0);

    //BehaviourTree Variables
    private int CurrentState = 0;

    //Behaviour States
    //0 = Idle
    //1 = Healing
    //2 = Crying
    //3 = Combining Powers
    //4 = Attacking Player
    //5 = Searching


    //Initialization
    protected override void Awake()
    {
        //Execute Parent Awake
        base.Awake();

        //Identify Mother AI
        IdentityMotherAI = GameObject.FindGameObjectWithTag("MotherAI");
        //Set Begin Values
        RangedAttack.SetActive(false);
        CloseAttack.SetActive(false);
        //Init AudioSource
        AS = GetComponent<AudioSource>();
    }


    //Update every frame
    protected override void Update ()
    {
        //Execute Parent Update
        base.Update();

        //BehaviourTree 
        BT();

        //States Execution
        if ( CurrentState == 1) { HealState(); }
        if (CurrentState == 4) { AttackPlayer(); }

        //Cooldowns
        if (HasCooldown)
        {
            Cooldown -= 1 * Time.deltaTime;
            if(Cooldown <= 0)
            {
                Cooldown = 0;
                HasCooldown = false;
            }
        }
    }

    //AI Die State
    protected override void Die()
    {
        print("ChildAI: " + gameObject.name + " Dies");
        //Delete this GameObject
        Destroy(gameObject);
    }

    //BehaviourTree
    private void BT()
    {
        //First Check HP
        if (!CheckHP())
        {
            //Forget Player
            IdentityPlayer = null;
            //If already found Mother AI
            if (NearMother)
            {
                //Stop Moving
                AtFocusLocation = true;
                //Ask mother to heal
                if (IdentityMotherAI != null && CurrentState != 1)
                {
                    //Ask Mother to heal Audio
                    AS.PlayOneShot(HealMeSound);
                    MotherAI MAI = IdentityMotherAI.GetComponent<MotherAI>();
                    //if request is accepted go into healing state
                    if (MAI.RequestHeal(this))
                    {
                        CurrentState = 1;
                    }
                }
            }
            //Try to find Mother AI
            else
            {
                FindMother();
            }
        }
        //Find Player if not healing
        else if(CurrentState != 1)
        {
            //If already found player
            if (NearPlayer)
            {
                //Stop Moving
                AtFocusLocation = true;
                //Look at player
                IdentityPlayer = GameObject.FindGameObjectWithTag("Player");
                Vector3 LookAtPos = IdentityPlayer.transform.position;
                LookAtPos.y = transform.position.y;
                gameObject.transform.LookAt(LookAtPos);

                //If there is a potentional ChildAI near
                if (NearChild && !HasCooldown)
                {
                    //Combine powers
                    CombinePowers();
                }
                //Else attack player
                else
                {
                    CurrentState = 4;
                }
            }
            //Find Player
            else
            {
                FindPlayer();
            }
        }
    }

    //Check if to retreat
    private bool CheckHP()
    {
        if(CurrentHealth > DangerHP)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Try to find MotherAI
    private void FindMother()
    {
        //If still not found Mother
        if (CheckAtLocation(RandomPos) && !NearMother)
        {
            //Cry
            Cry();
            //Try another search location
            RandomPos = RandomDestination();
        }
        //Moving to search location
        if (!CheckAtLocation(RandomPos))
        {
            //Keep Looking
            MoveToPosition(RandomPos);
        }
    }

    //In Healing State
    private void HealState()
    {
        //Stop healing when the mother walks away
        if (!NearMother)
        {
            CurrentState = 0;
        }
    }

    //In Crying State
    private void Cry()
    {
        //Play Crying Sound
        AS.PlayOneShot(CrySound);
    }

    //Try to find Player
    private void FindPlayer()
    {
        //If still not found Player
        if (CheckAtLocation(RandomPos) && !NearPlayer)
        {
            //Try another search location
            RandomPos = RandomDestination();
        }
        //Moving to search location
        if (!CheckAtLocation(RandomPos))
        {
            //Keep Looking
            MoveToPosition(RandomPos);
        }
    }

    //Try to find Child
    private void FindChild()
    {
        //If still not found Player
        if (CheckAtLocation(RandomPos) && !NearChild)
        {
            //Try another search location
            RandomPos = RandomDestination();
        }
        //Moving to search location
        if (!CheckAtLocation(RandomPos) )
        {
            //Keep Looking
            MoveToPosition(RandomPos);
        }
    }

    //Identify if the other ChildAI is potentional
    private void IdentifyChild(GameObject Target)
    {
        if (!HasCooldown)
        {
            ChildAI CAI = Target.GetComponent<ChildAI>();

            if (CAI != null)
            {
                if (!CAI.HasCooldown) { NearChild = true; }
            }
        }
    }

    //In Combine Powers State
    private void CombinePowers()
    {
        //Set Combine Powers Cooldown
        Cooldown = CombineCD;
        HasCooldown = true;
        StartCoroutine(RangedAttackTimer());
    }

    //Ranged Attack Duration Timer
    private IEnumerator RangedAttackTimer()
    {
        HasRangedAttack = true;
        yield return new WaitForSeconds(RangedAttackTime);
        HasRangedAttack = false;
    }

    //In Attack Player State
    private void AttackPlayer()
    {
        //Set ChildAI Attacking State
        if (HasRangedAttack)
        {
            RangedAttack.SetActive(true);
            CloseAttack.SetActive(true);
        }
        else
        {
            RangedAttack.SetActive(false);
            CloseAttack.SetActive(true);
        }

        //Follow Player if found player
        if (IdentityPlayer != null)
        {
            //Moving to Player
            if (!CheckAtLocation(IdentityPlayer.transform.position))
            {
                MoveToPosition(IdentityPlayer.transform.position);
            }
        }
    }

    //Sensor for entering objects
    private void OnTriggerEnter(Collider Target)
    {
        //Identify entered object
        switch (Target.gameObject.tag)
        {
            case "Player": NearPlayer = true; AS.PlayOneShot(HeySound); break;
            case "MotherAI": NearMother = true; break;
            case "ChildAI": IdentifyChild(Target.gameObject); break;
        }
    }

    //Sensor for exiting object
    private void OnTriggerExit(Collider Target)
    {
        //Identify exited object
        switch (Target.gameObject.tag)
        {
            case "Player": NearPlayer = false; break;
            case "MotherAI": NearMother = false;  break;
            case "ChildAI": NearChild = false; break;
        }
    }

}//CLASS
