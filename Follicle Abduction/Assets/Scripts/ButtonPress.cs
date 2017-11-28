using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
	public Collider collider;

	[SerializeField]
	Camera playerCamera;

	ButtonAnimation buttonAnimation;

	private bool isPressed; 

	void Start ()
	{
		buttonAnimation = GetComponent<ButtonAnimation> ();
	}

	void Update ()
	{

		//Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 10, Color.green);

		if (Input.GetKeyDown(KeyCode.E))
		{
			int layerMask = 1;  //Rays only hit objects on default layer

			RaycastHit hit;
			if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 5.0f, layerMask))
			{
				if (hit.collider == collider)
				{
					push ();
				}
			}
		}


	}

	void push()
	{
		isPressed = true;
		buttonAnimation.push ();

	}


	public bool getButtonStatus() 
	{

		return isPressed;
        
	}

    public void stopPress()
    {
        isPressed = false;
    }
}
