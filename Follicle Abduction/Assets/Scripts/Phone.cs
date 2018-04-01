using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Component for phone behaviour.
	Phone NODE can be unlocked by human player. Then the node is accessed by the alien player.
 */
public class Phone : MonoBehaviour
{

	private Node node;
	public GameObject phone;
	private bool touchingPlayer;

	void Start ()
	{
		node = GetComponentInChildren<Node> ();
	}
	
	void Update ()
	{
		if (touchingPlayer) {
			print (node.state);
			if (Input.GetKeyDown (KeyCode.E) && (node.state == NodeState.LOCKED)) {
				TurnOn ();
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		print (other.tag);
		if (other.CompareTag ("playerA")) {
			touchingPlayer = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.CompareTag ("playerA")) {
			touchingPlayer = false;
		}
	}

	void TurnOn ()
	{
		node.unlockNode ();
	}
		
}
