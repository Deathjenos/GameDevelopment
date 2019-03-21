using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIBasics : MonoBehaviour {

    //AI Health
    public float MaxHealth = 100.0f;
    public float CurrentHealth;
    public Slider SS;
    protected AudioSource AS;
    public AudioClip Hit;

    //Movement
    public float MovementSpeed = 5.0f;
    //Pathfinding
    private Pathfinding PF;
    private List<Node> Path;
    //Arrived Destination
    private Vector3 Focus;
    protected bool AtFocusLocation = true;
    public float DestinationRadius = 1.0f;


    //Initialization
    protected virtual void Awake () {
        //Set Start HP
        CurrentHealth = MaxHealth;
        //Initialize Pathfinding Mechanism
        PF = gameObject.GetComponent<Pathfinding>();
        if(PF == null)
        {
            PF = gameObject.AddComponent<Pathfinding>();
        }
        PF.StartPosition = gameObject.transform;
        PF.TargetPosition = gameObject.transform;
        //Set UI Visuals
        SetSliderVisuals();
    }
	
	//Update every frame
	protected virtual void Update () {
        //Moveing
        if (!AtFocusLocation)
        {
            if (Path != null)
            {
                //Move and Look On Path
                Vector3 TargetPosition = new Vector3(Path[0].Position.x, gameObject.transform.position.y, Path[0].Position.z);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, TargetPosition, MovementSpeed * Time.deltaTime);
                gameObject.transform.LookAt(TargetPosition);
                //At Final Location?
                AtFocusLocation = CheckAtLocation(Focus);
            }
        }
	}

    //Move to Object location with pathfinding
    protected void MoveToObject(GameObject TargetObject)
    {
        //Set Target Position to Pathfinding
        PF.TargetPosition = TargetObject.transform;
        //Find Path
        PF.FindPath(gameObject.transform.position, TargetObject.transform.position);
        //Get Final Path
        Path = PF.FinalPath;
        //Set Focus
        Focus = TargetObject.transform.position;
        //Check if at focus destination
        AtFocusLocation = CheckAtLocation(Focus);
    }

    //Move to Location with pathfinding
    protected void MoveToPosition(Vector3 TargetPos)
    {
        //Find Path
        PF.FindPath(gameObject.transform.position, TargetPos);
        //Get Final Path
        Path = PF.FinalPath;
        //Set Focus
        Focus = TargetPos;
        //Check if at focus destination
        AtFocusLocation = CheckAtLocation(Focus);
    }

    //Check if near focus Object
    protected bool CheckAtLocation(GameObject TargetObject)
    {
        float DistanceToLocation = Vector3.Distance(gameObject.transform.position, TargetObject.transform.position);

        bool AtLocation = false;
        if (DistanceToLocation <= DestinationRadius)
        {
            AtLocation = true;
        }
        else
        {
            AtLocation = false;
        }

        return AtLocation;
    }

    //Check if near focus Position
    protected bool CheckAtLocation(Vector3 TargetPosition)
    {
        float DistanceToLocation = Vector3.Distance(gameObject.transform.position, TargetPosition);

        bool AtLocation = false;
        if (DistanceToLocation <= DestinationRadius)
        {
            AtLocation = true;
        }
        else
        {
            AtLocation = false;
        }

        return AtLocation;
    }

    //AI Die State
    protected virtual void Die()
    {
        print("AIBasics: " + gameObject.name + " Dies");
        //Delete this GameObject
        Destroy(gameObject);
    }

    //Increase or Decrease CurrentHealth
    public void TranslateHP(float amount)
    {
        //Calculate new Current Health
        CurrentHealth += amount;

        //Die when Current Health is empty
        if(CurrentHealth <= 0)
        {
            Die();
        }
        //Check Full HP
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        //Play Hitting Sound when Decreasing Health
        if (amount < 0)
        {
            AS.PlayOneShot(Hit);
        }

        //Set Health Visuals to UI
        SetSliderVisuals();
    }

    //Set Health Visuals to UI
    private void SetSliderVisuals()
    {
        float procent = CurrentHealth / MaxHealth;
        SS.value = procent;
    }

    //Choose Random Destination
    protected Vector3 RandomDestination()
    {
        Node RandomNode = PF.grid.GetRandomNode();

        if(RandomNode != null)
        {
            return RandomNode.Position;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }

    }

}//CLASS
