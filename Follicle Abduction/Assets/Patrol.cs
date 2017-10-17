using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

	public Vector3[] navMesh;

	private UnityEngine.AI.NavMeshAgent agent;
	private int currentWaypoint = 0;
	
	private float StoppingDistance = 0.5f;

	void NavigateToNextWaypoint() {
		agent.isStopped = false;
		
		// Choose a new waypoint
		currentWaypoint = (currentWaypoint + 1) % navMesh.Length;
		agent.SetDestination(navMesh[currentWaypoint]);

		Debug.Log(agent.destination);
	}

	void Start () {
		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
		NavigateToNextWaypoint();
	}
	
	void Update () {
		if(Vector3.Distance(transform.position, navMesh[currentWaypoint]) < StoppingDistance) {
			NavigateToNextWaypoint();
		} 
	}
}
