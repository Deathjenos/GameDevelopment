using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {

    //CameraRotation Settings
	public float Y = 0.0f;
	public float AngleSpreadY = 90.0f;


	//Update every frame
	void Update () {
		CameraRotate ();
	}

    //Set Camera Rotation
	void CameraRotate(){
	    //Get 2D MousePosition
		Vector3 Mouse = Camera.main.ScreenToViewportPoint (Input.mousePosition);

        //Calculate 2D MousePosition to 3D Rotation
		if (Mouse.y > 0.5f && Mouse.y <= 1.0f) {
			Y = 0.0f + (-1 * ((Mouse.y - 0.5f) * (AngleSpreadY * 2)));
		} 
		else if (Mouse.y < 0.5f && Mouse.y >= 0.0f) {
			Y = -1 * ((Mouse.y - 0.5f) * (AngleSpreadY * 2));
		} 
		else if (Mouse.y == 0.5f) {
			Y = 0.0f;
		}
			
        //Set Camera Y Rotation
		transform.localEulerAngles = new Vector3 (Y, 0, 0);
	}

}//CLASS
