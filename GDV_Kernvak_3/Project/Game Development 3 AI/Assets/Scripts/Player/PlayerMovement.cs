using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {

    //Player Movement Settings
	public float speed = 1f;
	public float rotationSpeed = 10f;
	public float jumpHeight = 5f;
	public bool MayMove = true;

    //PlayerMovement Variables
	private Vector3 mouseStartX;
	private float CurrentMouseX;
    private bool CursorVisible = false;
    //PlayerMovement Calculations
    public float Y;
	public float Z;
	public float X;
    private bool onGround = true;
    private Rigidbody rb;


	//Initialization
	void Awake(){
        //Component initializiations
		rb = GetComponent<Rigidbody> ();
	}

	//Update every frame
	void Update () {
		Move ();
		CheckCursor ();
	}

    //Moving
	void Move(){
		if (MayMove) {
            //Position Translation Calculation
			Z = Input.GetAxisRaw ("Vertical") * speed * Time.deltaTime;
			X = Input.GetAxisRaw ("Horizontal") * speed * Time.deltaTime;
			transform.Translate (X, 0, Z);
            //Rotation Calculation
			float xMouse = CrossPlatformInputManager.GetAxis ("Mouse X") * rotationSpeed;
			Y = transform.localEulerAngles.y + xMouse;
			transform.localEulerAngles = new Vector3 (0, Y, 0);
            //May jump if on the ground
			if (onGround) {
				if (Input.GetKey (KeyCode.Space)) {
					rb.velocity = new Vector3 (0, jumpHeight, 0);

					onGround = false;
				}
			}
		}
	}

    //Set the cursor
	void CheckCursor(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CursorVisible = true;
		} 
		if(Input.GetMouseButtonDown(0)) {
			CursorVisible = false;
		}

		Cursor.visible = CursorVisible;
	}

    //On Collision Enter
	void OnCollisionEnter(Collision target){
        //Check ground collision
		if (target.gameObject.tag == "Ground") {
			onGround = true;
		}
	}

}//CLASS
