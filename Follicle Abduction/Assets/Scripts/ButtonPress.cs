using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
	Component that handles button being pressed.
	Handles raycast from human player and button animation.
	TODO: generalize! Many objects could have a checkraycast->animate behaviour
 */

public class ButtonPress : MonoBehaviour
{
	Collider buttonHitbox;
	Animation buttonAnimation;
	private bool isPressed;

	void Start ()
	{
		buttonAnimation = GetComponent<Animation> ();
		buttonHitbox = GetComponentInChildren<Collider>();
	}

	void Update ()
	{
		//Check for raycast when interact button "E" is pressed
		if (Input.GetKeyDown (KeyCode.E)) {
			int layerMask = 1;  //Rays only hit objects on default layer

			RaycastHit hit;
			GameObject player = GameObject.FindGameObjectWithTag ("playerA");
			Camera playerCamera = player.GetComponentInChildren<Camera>();
			if (Physics.Raycast (playerCamera.transform.position, playerCamera.transform.forward, out hit, 5.0f, layerMask)) {
				if (hit.collider == buttonHitbox) {
					push ();
				}
			}
		}
	}

	void push ()
	{
		isPressed = true;
		buttonAnimation.Play("pushed");
	}

	public bool getButtonStatus ()
	{
		return isPressed;
	}

	public void stopPress ()
	{
		isPressed = false;
	}
}
