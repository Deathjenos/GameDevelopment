using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour {

    //Move Boundaries
    public Vector3 MinPos;
    public Vector3 MaxPos;
    //Move Direction
    public bool MoveOnX = false;
    public bool MoveOnY = false;
    public bool MoveOnZ = false;
    //Move Speed
    public float Speed = 5f;
    private float Direction = 1f;
    private bool ChangeDirection = false;


	//Update every frame
	void Update () {
        //Move Wall
        Move();
	}

    //Move Wall
    private void Move()
    {
        //Define Direction and Speed
        float XYZ = Direction * Speed * Time.deltaTime;
        float X = transform.position.x;
        float Y = transform.position.y;
        float Z = transform.position.z;
        Vector3 NewPos = transform.position;

        //Calculate new Position
        if (MoveOnX)
        {
            NewPos.x = X + XYZ;
            if(NewPos.x <= MinPos.x)
            {
                ChangeDirection = true;
                NewPos.x = MinPos.x;
            }
            if(NewPos.x >= MaxPos.x)
            {
                ChangeDirection = true;
                NewPos.x = MaxPos.x;
            }
        }
        if (MoveOnY)
        {
            NewPos.y = Y + XYZ;
            if (NewPos.y <= MinPos.y)
            {
                ChangeDirection = true;
                NewPos.y = MinPos.y;
            }
            if (NewPos.y >= MaxPos.y)
            {
                ChangeDirection = true;
                NewPos.y = MaxPos.y;
            }
        }
        if (MoveOnZ)
        {
            NewPos.z = Z + XYZ;
            if (NewPos.z <= MinPos.z)
            {
                ChangeDirection = true;
                NewPos.x = MinPos.x;
            }
            if (NewPos.z >= MaxPos.z)
            {
                ChangeDirection = true;
                NewPos.z = MaxPos.z;
            }
        }

        //Check if change direction
        if (ChangeDirection)
        {
            Direction *= -1;
            ChangeDirection = false;
        }

        //Transform Position
        transform.position = NewPos;
    }

}//CLASS
