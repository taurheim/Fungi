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
	public ArrayList secondaryTargets; //distracting environmental things that can't be captured (ringing phones, music, etc.)

    public Vector3 currDestination;
    public GameObject currChaseTarget;
    public Vector3 lastKnownTargetLoc;

    void NavigateToNextWaypoint() {

        // Choose a new waypoint
        currentWaypoint = (currentWaypoint + 1) % navMesh.Length;
        currDestination = navMesh[currentWaypoint];
    }

    void Start() {
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
		secondaryTargets = new ArrayList ();
        NavigateToNextWaypoint();
    }

    void Update() {
        // If currently chasing a target
        if (currChaseTarget != null) {

            // If target is within reach, capture and resume patrolling
			if (Vector3.Distance (this.transform.position, currChaseTarget.transform.position) < 2) {
				if (currChaseTarget.CompareTag ("playerA")) {
					Capture (currChaseTarget);
					ResumePatrol ();
				} else {
					//do nothing! just stay put
				}
			} 
			//If a secondaryTarget (not player) we assume it is a sound? so dont need detect... will clean up this later
			else if (!currChaseTarget.CompareTag ("playerA")) {
				lastKnownTargetLoc = currChaseTarget.transform.position;
				currDestination = currChaseTarget.transform.position;
			}

            // Check if target is still in sight, if so, note location and keep chasing
            else if (Detect(currChaseTarget)) {
                lastKnownTargetLoc = currChaseTarget.transform.position;
                currDestination = currChaseTarget.transform.position;
            }

            // If patroller reaches last known location, give up and return to patrolling
            else if (Vector3.Distance(this.transform.position, lastKnownTargetLoc) < 1.5) {
                ResumePatrol();
            }

            // If sight of target has been lost, move towards last known location
            else {
                currDestination = lastKnownTargetLoc;
            }
        }

        // If not currently chasing, simply patrol to next waypoint
        else {
            if (Vector3.Distance(transform.position, navMesh[currentWaypoint]) < StoppingDistance) {
                NavigateToNextWaypoint();
            }

            // Look for all targets in target array, if any found, begin chase
			bool detectedPrimaryTarget = false;
            foreach (GameObject target in detectTargets) {
                if (Detect(target)) {
                    Chase(target);
					detectedPrimaryTarget = true;
                }
            }
			if (!detectedPrimaryTarget) {
				foreach (GameObject target in secondaryTargets) {
					Chase (target);
				}
			}
        }

        // Might have to reduce how often this is called (only when destination changes)
        MoveToDestination();
    }

    void MoveToDestination() {
        if (agent != null) {
            agent.isStopped = false;
            agent.SetDestination(currDestination);
        }
    }

    // Check for target within cone of sight
    public bool Detect(GameObject target) {

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
   public void Capture(GameObject target) {
	        // Placeholder effect, not sure how to do gameover yet
        if (target.transform.parent != null) {
            target.transform.parent.gameObject.transform.position = new Vector3(-11.8f, -11.01f, 0.7f);
        }
        else {
            target.transform.position = new Vector3(-11.8f, -11.01f, 0.7f);
        }
        currChaseTarget = null;
    }

    void Chase(GameObject target) {
        currChaseTarget = target;
        currDestination = target.transform.position;

        // Increase agility, zoomzoom
        agent.angularSpeed = 500;
        agent.speed = 5f;
    }

    void ResumePatrol() {
        currChaseTarget = null;

        currDestination = navMesh[currentWaypoint];

        // Return to normal patrol speed
        if (agent != null) {
            agent.angularSpeed = 120;
            agent.speed = 3.5f;
        }
    }

	public void AddSecondaryTarget(GameObject newTarget) {
		print (secondaryTargets);
		secondaryTargets.Add (newTarget);
	}

	public void RemoveSecondaryTarget(GameObject target) {
		if (target == currChaseTarget) {
			currChaseTarget = null;
			ResumePatrol ();
		}
		secondaryTargets.Remove (target);
	}
}
