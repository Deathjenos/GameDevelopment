using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour {

    //Attack Stats
    public float AttackDamage = 1.0f;


    //On Collision Enter
    private void OnCollisionEnter(Collision Target)
    {
        //Deal damage to Player if hitting Player
        if(Target.gameObject.tag == "Player")
        {
            PlayerScript PS = Target.gameObject.GetComponent<PlayerScript>();
            float Damage = AttackDamage * -1;
            PS.TranslateHP(Damage);
        }
    }

}//CLASS
