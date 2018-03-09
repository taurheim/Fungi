using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
	public Collider buttonHitbox;

	ButtonAnimation buttonAnimation;

	private bool isPressed;

	void Start ()
	{
		buttonAnimation = GetComponent<ButtonAnimation> ();
	}

	void Update ()
	{
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
		buttonAnimation.push ();
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
