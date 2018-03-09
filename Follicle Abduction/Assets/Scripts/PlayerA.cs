using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Human player, handles pickups.
 */
public class PlayerA : MonoBehaviour
{

	private int pickups = 0;

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("pickup")) {
			other.gameObject.SetActive (false);
			pickups++;
			print ("pickups: " + pickups);
		}
	}
}
