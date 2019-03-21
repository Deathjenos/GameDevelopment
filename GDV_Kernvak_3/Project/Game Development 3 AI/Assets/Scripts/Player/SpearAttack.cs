using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearAttack : MonoBehaviour {

    //Spear Stats
    public float AttackDamage = 10.0f;

    //Spear Variables
    private Animator AM;
    public GameStateManager GSM;

	//Initialization
	void Awake () {
        AM = GetComponent<Animator>();
	}
	
	//Update every frame
	void Update () {
        //Attack with left mouseclick
        if (Input.GetMouseButtonDown(0))
        {
            AM.SetBool("Attack", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            AM.SetBool("Attack", false);
        }
    }

    //On Collision Enter
    private void OnCollisionEnter(Collision Target)
    {
        //Deal Damage to ChildAI if hitting ChildAI
        if(Target.gameObject.tag == "ChildAI")
        {
            ChildAI CAI = Target.gameObject.GetComponent<ChildAI>();
            float Damage = AttackDamage * -1;
            CAI.TranslateHP(Damage);
        }
        //Deal Damage to MotherAI if hitting MotherAI and no ChildAI's are alive
        if (Target.gameObject.tag == "MotherAI" && GSM.AmountOfChildAI == 0)
        {
            MotherAI MAI = Target.gameObject.GetComponent<MotherAI>();
            float Damage = AttackDamage * -1;
            MAI.TranslateHP(Damage);
        }
    }

}//CLASS
