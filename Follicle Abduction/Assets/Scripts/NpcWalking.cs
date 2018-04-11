using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Allow for entity to have waypoints which are sequentially visited.
	Entity will attempt to move through all positions in navMesh before looping backwards.
	Used to animate non-interactive NPCs along set paths.

 */
public class NPCWalking : MonoBehaviour
{

    public Vector3[] navMesh;

    public bool Out;

    private UnityEngine.AI.NavMeshAgent agent;
    private int currentWaypoint = -1;
    private float StoppingDistance = 0.5f;

    public Vector3 currDestination;

    public GameObject artModel;
    private string currAnimation;
    public Vector3 sendToOnCapture;

    void NavigateToNextWaypoint()// This moves the npc through its own set destination points in a loop
                                 // this is here because the guards need to path in a predicable manner
    {

        // Choose a new waypoint
        currentWaypoint = (currentWaypoint + 1) % navMesh.Length;
        /*
            if (currentWaypoint == 2 && !Out)
        {
            transform.position = new Vector3(-33, -14, 27);
        }
        if (currentWaypoint == 2 && Out)
        {
            transform.position = new Vector3(-3, -14, 43);
        }
        */
        currDestination = navMesh[currentWaypoint];
    }


    // Use this for initialization
    void Start()// Needed because guards may not always spawn in the same location, this accomidates for that by starting them
                // on the correct first destination not to start its loop
    {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        currAnimation = "";
        if (transform.position.x < -3 && transform.position.x > -32 && !Out)
        {
            currentWaypoint = 2;
        }
        if (transform.position.x > -3 && !Out)
        {
            currentWaypoint = 0;
        }
        if (transform.position.x < -32 && !Out)
        {
            currentWaypoint = 1;
        }
        if (transform.position.x < -3 && transform.position.x > -32 && Out)
        {
            currentWaypoint = 2;
        }
        if (transform.position.x > -3 && Out)
        {
            currentWaypoint = 1;
        }
        if (transform.position.x < -32 && Out)
        {
            currentWaypoint = 0;
        }

        NavigateToNextWaypoint();
        //artModel.GetComponent<Animation>().Play("walk_cycle", PlayMode.StopAll);
    }

    // Update is called once per frame
    void Update()
    {
        // instead of having pathing, where the player can't see them, 
        // it was better for optization to have the guards teleport to next visible 
        // location, and reduce the total number of NPCs

        if (Vector3.Distance(transform.position, navMesh[currentWaypoint]) < StoppingDistance)
        {
            if (currentWaypoint == 1 & !Out)
                transform.position = new Vector3(-33, -14, 27);

            if (currentWaypoint == 1 & Out)
                transform.position = new Vector3(-3, -14, 42);

            NavigateToNextWaypoint();

        }
        MoveToDestination();
        //artModel.GetComponent<Animation>().Play("walk_cycle");

    }


    void MoveToDestination()
    {
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(currDestination);

        }
    }
}
