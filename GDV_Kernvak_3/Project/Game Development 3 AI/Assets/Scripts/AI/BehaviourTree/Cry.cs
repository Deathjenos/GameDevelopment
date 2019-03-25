using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cry : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private AudioSource AS;
    private AudioClip CrySound;

    //Constructor
    public Cry(BlackBoard BB)
    {
        this.BB = BB;
        AS = BB.GetValue<AudioSource>("AS");
        CrySound = BB.GetValue<AudioClip>("CrySound");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Cry
        AS.PlayOneShot(CrySound);

        return TaskStatus.Completed;
    }

}//CLASS
