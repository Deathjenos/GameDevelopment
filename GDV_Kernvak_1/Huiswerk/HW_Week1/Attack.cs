using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public float speed = 0.1f;

	private GameObject camera;
	private CameraRotation cameraRot;
 	private GameObject player;
	private PlayerMovement playerPM;
	private Animator anim;

	private GameObject Dimension;
	private DimensionState DS;

	private float X;
	private float Y;

	private bool MayMove = true;


	public void Awake(){
		camera = GameObject.Find ("Camera");
		player = GameObject.Find ("Player");
		Dimension = GameObject.Find ("DimensionState");
		cameraRot = camera.GetComponent<CameraRotation> ();
		playerPM = player.GetComponent<PlayerMovement> ();
		anim = GetComponent<Animator> ();
		X = camera.transform.localEulerAngles.x;
		Y = player.transform.localEulerAngles.y;
		DS = Dimension.GetComponent<DimensionState> ();

		if (DS.BigStar) {
			anim.Play ("BigStarMove");
		}
	}


	// Update is called once per frame
	public void Update () {
		Move ();
	}


	public void Move(){
		if (MayMove) {
			transform.localEulerAngles = new Vector3 (X, Y, 0);
			transform.Translate (0, 0, speed);
		}
	}


	public void StarDead(){
		StartCoroutine (Dead ());
	}


	public IEnumerator Dead(){
		MayMove = false;
		anim.SetBool ("Die", true);
		yield return new WaitForSeconds (0.5f);
		Destroy (gameObject);
	}
}
