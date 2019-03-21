using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToCamera : MonoBehaviour {

    //Camera to rotate to
    private Camera CameraToLookAt;


    //Initilaization
    private void Awake()
    {
        //Find Main Camera
        CameraToLookAt = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    //Update every frame
    void Update () {
        //Calculate Counter Rotation
        Vector3 Y = CameraToLookAt.transform.position - transform.position;
        Y.x = 0.0f;
        Y.z = 0.0f;
        //Set Counter Rotation
        transform.LookAt(CameraToLookAt.transform.position - Y);
        transform.Rotate(0, 180, 0);
	}

}//CLASS
