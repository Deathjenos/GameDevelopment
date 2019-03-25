using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {

    //GameStateManager Library
    GameObject[] ChildAIList;
    public int AmountOfChildAI = 0;
    GameObject MotherAI;
    PlayerScript PS;
    public Text tt;
    public float WaitTimeToMenu = 5.0f;
    public GameObject Win;
    public GameObject Lose;


	//Initialization
	private void Awake () {
        //Find MotherAI
        MotherAI = GameObject.FindGameObjectWithTag("MotherAI");
        //Find Player
        PS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        //Starting state
        Win.SetActive(false);
        Lose.SetActive(false);
    }
	
	//Update every frame
	private void Update () {
        //Update Amount of ChildAI's
        FindChildObjects();
        //Update Game State
        ///If Lost
        if(PS.CurrentHP == 0)
        {
            Lose.SetActive(true);
            StartCoroutine(WaitToMenu());
        }
        ///If Won
        else if(MotherAI == null)
        {
            Win.SetActive(true);
            StartCoroutine(WaitToMenu());
        }
	}

    //Find all ChildAI's
    private void FindChildObjects()
    {
        //Find ChildAI's
        ChildAIList = GameObject.FindGameObjectsWithTag("ChildAI");
        //Set Amount of ChildAI's
        AmountOfChildAI = ChildAIList.Length;
        //Set Amount of ChildAI's to UI
        string Text = "Children left: " + AmountOfChildAI.ToString();
        tt.text = Text;
    }

    //Load and Set Scene to Menu
    private void GoToMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    //Wait to GoToMenu
    private IEnumerator WaitToMenu()
    {
        yield return new WaitForSeconds(WaitTimeToMenu);
        GoToMenu();
    }

    //On Collision Enter
    private void OnCollisionEnter(Collision Target)
    {
        //Player loses when out of level
        if(Target.gameObject.tag == "Player")
        {
            Lose.SetActive(true);
            StartCoroutine(WaitToMenu());
        }
    }

}//CLASS
