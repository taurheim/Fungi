﻿

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
	Component for human-behaviour, besides the FPS controller.
	Currently handles:
		- pickups
		- resetting position 
 */

public class HumanPlayer : NetworkedObject {

	private double minYPosition; // Player out of bounds below this height!
	public int pickups{
		get; private set;
	}
	private Vector3 originalPosition;

    GameOver gameOver;

	void Awake () {
		pickups = 0;
		originalPosition = transform.position;
		GameObject floor = GameObject.Find("Floor");
		minYPosition = floor.transform.position.y - 100.0;

        gameOver = FindObjectOfType<GameOver>();
	}
	
	void Update () {
		if (transform.position.y < minYPosition){
            gameOver.gameOver();
			//ResetPosition();
		}
	}

	void OnTriggerEnter (Collider other)
	{
		HandleTrigger(other);
	}

	//Created separate function so that it can be called for unit testing
	//Not sure how to test OnTriggerEnter with unit testing otherwise...
	public void HandleTrigger (Collider other)
	{
		// Pick up objects with "pickup" tag!
		if (other.gameObject.CompareTag ("pickup")) {
			other.gameObject.SetActive (false);
			pickups++;
		}
	}

	public void ResetPosition() {
		NetworkInteract();
	}

	protected override void Interact(){
		transform.position = originalPosition;
	}
}
