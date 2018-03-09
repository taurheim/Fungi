using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : MonoBehaviour {

	public int pickups{
		get; private set;
	}
	private Vector3 originalPosition;

	// Use this for initialization
	void Awake () {
		pickups = 0;
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -55.0){
			ResetPosition();
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
		transform.position = originalPosition;
	}
}
