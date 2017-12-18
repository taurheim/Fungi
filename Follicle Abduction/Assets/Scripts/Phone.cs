using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Custom behaviour for the phone.
 */
public class Phone : MonoBehaviour {

	private Node node;
	public GameObject phone;
	private bool touchingPlayer;

	// Use this for initialization
	void Start () {
		node = GetComponentInChildren<Node> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (touchingPlayer) {
			print (node.state);
			if (Input.GetKeyDown (KeyCode.E) && (node.state == NodeState.LOCKED)) {
				TurnOn ();
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		print (other.tag);
		if (other.CompareTag ("playerA")) {
			touchingPlayer = true;
			print (touchingPlayer);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.CompareTag ("playerA")) {
			touchingPlayer = false;
		}
	}

	void TurnOn() {
		print ("TURNING ON!");
		//make some turning-on sound
		node.unlockNode ();
		//do some visual to show it's turned on
	}
		
}
