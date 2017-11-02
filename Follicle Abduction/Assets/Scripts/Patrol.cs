using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Patrol : MonoBehaviour {

	public Vector3[] navMesh;

	private UnityEngine.AI.NavMeshAgent agent;
	private int currentWaypoint = 0;
	
	private float StoppingDistance = 0.5f;

	// Cone of detection is determined by angle and length
	public float detectAngle;
	public float detectRange;

    // Array of target objects to look for and try to capture
	public GameObject[] detectTargets;

    private GameObject currChaseTarget;
    private Vector3 lastKnownTargetLoc;

	void NavigateToNextWaypoint() {
		agent.isStopped = false;
		
		// Choose a new waypoint
		currentWaypoint = (currentWaypoint + 1) % navMesh.Length;
		agent.SetDestination(navMesh[currentWaypoint]);

		Debug.Log(agent.destination);
	}

	void Start () {
		print ("GUARD LOADED");
		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
		NavigateToNextWaypoint();
	}

	void Update () {
        // If currently chasing a target
        if (currChaseTarget != null) {

            // If target is within reach, capture and resume patrolling
            if (Vector3.Distance(this.transform.position, currChaseTarget.transform.position) < 2) {
                Capture(currChaseTarget);
                currChaseTarget = null;
                agent.SetDestination(navMesh[currentWaypoint]);

                // Return to normal quickness
                agent.angularSpeed = 120;
                agent.speed = 3.5f;
            }

            // Check if target is still in sight, if so, note location and keep chasing
            else if (Detect(currChaseTarget)) {
                lastKnownTargetLoc = currChaseTarget.transform.position;
                agent.SetDestination(currChaseTarget.transform.position);
                
            }

            // If patroller reaches last known location, give up and return to patrolling
            else if (Vector3.Distance(this.transform.position, lastKnownTargetLoc) < 1.5) {
                currChaseTarget = null;
                agent.SetDestination(navMesh[currentWaypoint]);

                // Return to normal quickness
                agent.angularSpeed = 120;
                agent.speed = 3.5f;
            }

            // If sight of target has been lost, move towards last known location
            else {
                agent.SetDestination(lastKnownTargetLoc);
            }
        }

        // If not currently chasing, simply patrol to next waypoint
        else {
            if (Vector3.Distance(transform.position, navMesh[currentWaypoint]) < StoppingDistance) {
                NavigateToNextWaypoint();
            }

            // Look for all targets in target array, if any found, begin chase
            foreach (GameObject target in detectTargets) {
                if (Detect(target)) {
                    currChaseTarget = target;
                    agent.SetDestination(target.transform.position);

                    // Increase agility, zoomzoom
                    agent.angularSpeed = 500;
                    agent.speed = 5f;
                }
            }
        }
	}

	// Check for target within cone of sight
	bool Detect(GameObject target){

		// Check if target is within detect range
		if (Vector3.Distance(target.transform.position, this.transform.position) <= detectRange) {
			
			// Check if target is within detect angle
			if (Vector3.Angle(this.transform.forward, target.transform.position - this.transform.position) < detectAngle) {
				return true;
			}
		} 

		return false; 
	}

	// After successfully catching up to player, capture
	void Capture(GameObject target){

        // Placeholder effect, not sure how to do gameover yet
        target.transform.parent.gameObject.transform.position = new Vector3(-11.8f,-11.01f,0.7f);
    }
}
