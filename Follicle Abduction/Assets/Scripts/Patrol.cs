﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public enum AnimationState {
	STOPPED,
	WALKING,
	RUNNING,
}

/*
	Attach to guards to detect player and attempt to follow them when detected.
 */
public class Patrol : NetworkedObject
{
	public Vector3[] navMesh;

	private UnityEngine.AI.NavMeshAgent agent;
	private int currentWaypoint = 0;
	private float StoppingDistance = 0.5f;
	public float walkSpeed = 3.5f;
	public float runSpeed = 7.0f;
	public float waitTime = 10.0f;

	// Cone of detection is determined by angle and length
	public float detectAngle;
	public float detectRange;

	public GameObject[] detectTargets; // Array of target objects to look for and try to capture
	public List <GameObject> secondaryTargets = new List<GameObject> (); // Targets with lower priority (distractions)

	public Vector3 currDestination;
	public GameObject currChaseTarget;
	public Vector3 lastKnownTargetLoc;
	private Quaternion originalFaceDirection;
	private float remainingWaitDuration;

	public GameObject artModel;
	public Vector3 sendToOnCapture;

	[SyncVar]
	public AnimationState currentState;

	private AnimationState lastState;

    public GameOver gameOver;

	void NavigateToNextWaypoint ()
	{
		// Choose a new waypoint
		currentWaypoint = (currentWaypoint + 1) % navMesh.Length;
		currDestination = navMesh [currentWaypoint];
    }

	public override void Start ()
	{
		base.Start();
		agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
		secondaryTargets = new List<GameObject>();
		NavigateToNextWaypoint ();
        originalFaceDirection = this.transform.rotation;
        remainingWaitDuration = 0.0f;
    }

    void Update ()
	{
		if(currentState != lastState) {
			switch(currentState) {
				case AnimationState.STOPPED:
					artModel.GetComponent<Animation>().Play("look_around", PlayMode.StopAll);
				break;
				case AnimationState.WALKING:
					artModel.GetComponent<Animation>().Play("walk_cycle", PlayMode.StopAll);
				break;
				case AnimationState.RUNNING:
					artModel.GetComponent<Animation>().Play("run_cycle", PlayMode.StopAll);
				break;
			}
			lastState = currentState;
		}
		
		// Only run on the server
		if(!networkManager.isTheHost()) {
			return;
		}

		// Update animation state
		if (agent.velocity.magnitude == 0.0f) {
			currentState = AnimationState.STOPPED;
		} 
		else if (agent.speed == walkSpeed){
			currentState = AnimationState.WALKING;
		}
		else if (agent.speed > walkSpeed) {
			currentState = AnimationState.RUNNING;
		}

		// Check if the guard is currently waiting, if so, do not proceed
		if (remainingWaitDuration > 0.0f){
			remainingWaitDuration -= Time.deltaTime;

			// If the guard is no longer waiting after this frame, resume walking
			if (remainingWaitDuration <= 0.0f) {
				agent.Resume();
				NavigateToNextWaypoint();

			}
			return;
		}


		// If currently chasing a target
		if (currChaseTarget != null) {

			if (currChaseTarget.tag == ("playerA")) {
				if (Vector3.Distance (this.transform.position, currChaseTarget.transform.position) < 3.5f) {
					Capture (currChaseTarget);
					ResumePatrol();
				}

				// Check if target is still in sight, if so, note location and keep chasing
				else if (Detect (currChaseTarget)) {
					lastKnownTargetLoc = currChaseTarget.transform.position;
					currDestination = currChaseTarget.transform.position;
				}

				// If patroller reaches last known location, give up and return to patrolling
				else if (Vector3.Distance (this.transform.position, lastKnownTargetLoc) < 2.0f) {
					ResumePatrol();
				}

				// If sight of target has been lost, move towards last known location
				else {
					currDestination = lastKnownTargetLoc;
				}
			} else {
				//If a secondaryTarget (not player) we assume it is a sound? so dont need detect... will clean up this later
				if (Vector3.Distance (this.transform.position, currChaseTarget.transform.position) < 5.0f) {
					agent.isStopped = true;
					agent.speed = 0.0f;
					WaitAtDestination(waitTime);
				} else {
					lastKnownTargetLoc = currChaseTarget.transform.position;
					currDestination = currChaseTarget.transform.position;
				}
			}
		}

		// If not currently chasing, simply patrol to next waypoint
		else {
			if (Vector3.Distance (transform.position, navMesh [currentWaypoint]) < StoppingDistance) {
                // If this guard is meant to stay in one position (rather than patrolling)
                if (navMesh.Length == 1) {
                    this.transform.rotation = originalFaceDirection;
                }
                else {
                    NavigateToNextWaypoint();
                }
			}

			// Look for all targets in target array, if any found, begin chase
			bool detectedPrimaryTarget = false;
			detectTargets = GameObject.FindGameObjectsWithTag ("playerA");
			foreach (GameObject target in detectTargets) {
				if (Detect(target)) {
					Chase(target);
					detectedPrimaryTarget = true;

                    AudioSource sfx = GetComponent<AudioSource>();
                    sfx.Play();
                }
			}
			if (!detectedPrimaryTarget) {
				foreach (GameObject target in secondaryTargets) {
					Chase(target);
				}
			}
		}

		MoveToDestination();
	}

	// Sets destination
	void MoveToDestination()
	{
		if (agent != null) {
			agent.isStopped = false;
			agent.SetDestination (currDestination);
		}
	}

	// Check for target within cone of sight
	public bool Detect (GameObject target)
	{
		// Check if target is within detect range
		if (Vector3.Distance (target.transform.position, this.transform.position) <= detectRange) {

			// Check if target is within detect angle
			if (Vector3.Angle(this.transform.forward, target.transform.position - this.transform.position) < detectAngle) {
				RaycastHit hit;
				if (Physics.Raycast (this.transform.position, target.transform.position - this.transform.position, out hit, detectRange)) {
					Debug.Log (hit.collider.tag);
					if (hit.transform.tag == target.tag) {
						return true;
					}
				}
			}
		}

		return false;
	}

	// After successfully catching up to player, capture
	public void Capture(GameObject target)
	{
        //display game over screen
        gameOver.gameOver();

        target.GetComponent<HumanPlayer>().ResetPosition();
        currChaseTarget = null;
    }

	void Chase(GameObject target)
	{
		currChaseTarget = target;
		currDestination = target.transform.position;

		// Increase agility, zoomzoom
		agent.angularSpeed = 500;
		agent.speed = runSpeed;
	}

	void ResumePatrol()
	{
		currChaseTarget = null;

		currDestination = navMesh [currentWaypoint];

		// Return to normal patrol speed
		if (agent != null) {
			agent.angularSpeed = 120;
			agent.speed = walkSpeed;
		}
	}


	public void AddSecondaryTarget(GameObject newTarget)
	{
		secondaryTargets.Add(newTarget);
	}

	public void RemoveSecondaryTarget(GameObject target)
	{
		if (target == currChaseTarget) {
			currChaseTarget = null;
			ResumePatrol();
		}
		secondaryTargets.Remove(target);
	}

	public void WaitAtDestination(float duration){
		remainingWaitDuration = duration;
		agent.isStopped = true;
	}

}
