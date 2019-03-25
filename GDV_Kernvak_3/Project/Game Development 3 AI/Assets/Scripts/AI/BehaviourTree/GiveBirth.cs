using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBirth : BTNode
{
    //Node Stats
    private BlackBoard BB;
    private GameObject Womb;
    private WombScript WS;
    private AudioSource AS;
    private AudioClip ScreamSound;

    //Constructor
    public GiveBirth(BlackBoard BB)
    {
        this.BB = BB;
        Womb = BB.GetValue<GameObject>("Womb");
        WS = Womb.GetComponent<WombScript>();
        AS = BB.GetValue<AudioSource>("AS");
        ScreamSound = BB.GetValue<AudioClip>("ScreamSound");
    }

    //Run Method
    public override TaskStatus Run()
    {
        //Give Birth
        WS.GiveBirth();
        GiveBirthVisuals();

        return TaskStatus.Completed;
    }

    //GiveBirth State
    private void GiveBirthVisuals()
    {
        //Play Screaming Sound
        AS.PlayOneShot(ScreamSound);
    }

}//CLASS
