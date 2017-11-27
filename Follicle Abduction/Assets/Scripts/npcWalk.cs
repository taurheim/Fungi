using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class npcWalk : MonoBehaviour {

    public Vector3[] navMesh;

    private UnityEngine.AI.NavMeshAgent agent;
    private int currentWaypoint = -1;
    private float StoppingDistance = 0.5f;

    public Vector3 currDestination;

    public GameObject artModel;
    private string currAnimation;
    public Vector3 sendToOnCapture;

    void NavigateToNextWaypoint()
    {

        // Choose a new waypoint
        currentWaypoint = (currentWaypoint + 1) % navMesh.Length;
        currDestination = navMesh[currentWaypoint];
    }


    // Use this for initialization
    void Start () {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        currAnimation = "";
        if(transform.position.x < -32)
        {
            currentWaypoint = 1;
        }
        if (transform.position.x > -4)
        {
            currentWaypoint = 0;
        }
            NavigateToNextWaypoint();
        artModel.GetComponent<Animation>().Play("walk_cycle", PlayMode.StopAll);
    }
	
	// Update is called once per frame
	void Update () {


        if (Vector3.Distance(transform.position, navMesh[currentWaypoint]) < StoppingDistance)
        {
            NavigateToNextWaypoint();

            if (currentWaypoint == 2)
            {
                transform.position = new Vector3(-33, -14, 27);
            }
        }
        MoveToDestination();
        artModel.GetComponent<Animation>().Play("walk_cycle");

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
