using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

	public Vector3[] navMesh;

	private UnityEngine.AI.NavMeshAgent agent;
	private int currentWaypoint = 0;
	
	private float StoppingDistance = 0.5f;

	// Cone (pie slice, actually) is determined by angle and length
	public float detectAngle;
	public float detectRange;

	public GameObject[] detectTargets;

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
		if (Vector3.Distance (transform.position, navMesh [currentWaypoint]) < StoppingDistance) {
			NavigateToNextWaypoint ();
		}

		foreach (GameObject target in detectTargets) {
			if (Detect(target)){
				Chase(target);
			}
		}

	}

	// Check for player within cone of sight
	bool Detect(GameObject target){

		// Check if target is within detect range
		if (Vector3.Distance(target.transform.position, this.transform.position) <= detectRange) {
			
			// Check if target is within detect angle
			if (Vector3.Angle (this.transform.forward, target.transform.position - this.transform.position) < detectAngle) {
				Debug.Log("Detected!");
				return true;
			}
		} 

		return false; 
	}

	// After detecting, attempt to capture (by chasing)
	void Chase(GameObject target){
		Debug.Log("CHASING BOIS");
	}

	// After successfully catching up to player, capture
	void Capture(){
		
	}
}
