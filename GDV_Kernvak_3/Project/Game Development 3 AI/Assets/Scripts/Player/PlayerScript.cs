using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    //Player Stats
    public float MaxHP = 100.0f;
    public float CurrentHP = 100.0f;
    public float HealPerSecond = 1.0f;

    //Public Variables
    public Slider SS;
    private AudioSource AS;
    public AudioClip Scream;

	//Initialization
	private void Awake () {
        CurrentHP = MaxHP;
        SetSliderVisuals();
        //Init AudioSource
        AS = GetComponent<AudioSource>();
    }

    //Update every frame
    private void Update()
    {
        //Heal Regenation
        if(CurrentHP < MaxHP && CurrentHP > 0)
        {
            TranslateHP(HealPerSecond * Time.deltaTime);
        }
    }

    //Increase or Decrease CurrentHealth
    public void TranslateHP(float amount)
    {
        //Calculate new Current Health
        CurrentHP += amount;

        //Die when Current Health is empty
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
        }
        //Check Full HP
        if (CurrentHP >= MaxHP)
        {
            CurrentHP = MaxHP;
        }

        //Play Hit Audio when getting damage
        if (amount < 0)
        {
            AS.PlayOneShot(Scream);
        }
        //Set Health on UI
        SetSliderVisuals();
    }

    //Set Health on UI
    private void SetSliderVisuals()
    {
        //Calculate Health procent
        float procent = CurrentHP / MaxHP;
        //Set Health procent
        SS.value = procent;
    }

}//CLASS
