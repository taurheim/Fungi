using UnityEngine;
using System;
using System.Collections;

/*
	Make object float up and down above the ground.
	Simply a visual effect, currently used for pick-ups.
 */
public class Float : MonoBehaviour
{
	float originalY;

	// How much the object will fluctuate up and down (lower will stay more stationary)
	public float floatStrength = 1;

	void Start ()
	{
		this.originalY = this.transform.position.y;
	}

	void Update ()
	{
		transform.position = new Vector3 (transform.position.x,
			originalY + ((float)Math.Sin (Time.time) * floatStrength),
			transform.position.z);
	}
}