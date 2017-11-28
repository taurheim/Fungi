using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerA : MonoBehaviour {

	private int pickups;

	// Use this for initialization
	void Start () {
		pickups = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("pickup")) {
			other.gameObject.SetActive (false);
			pickups++;
			print ("pickups: " + pickups);
		}
	}
}
