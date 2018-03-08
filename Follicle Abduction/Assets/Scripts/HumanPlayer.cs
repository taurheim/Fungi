using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : MonoBehaviour {

	private int pickups;
	private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		pickups = 0;
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("pickup")) {
			other.gameObject.SetActive (false);
			pickups++;
		}
	}

	public void ResetPosition() {
		transform.position = originalPosition;
	}
}
