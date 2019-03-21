using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {

    //PlayButton Settings
	public string goToLevel = "0";


	//Initialization
	private void Start () {
        //Load and Set Scene
		SceneManager.LoadScene (goToLevel, LoadSceneMode.Single);
    }

}//CLASS
