using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Transform button;
    public Camera playerCamera;

    private bool isBeingPressed;    // true if the button is in the process of moving from a press

    private float buttonSpeed; 
    private float buttonMoveDistance;
    private Vector3 initialButtonPosition;

	void Start ()
    {
        isBeingPressed = false;
        buttonSpeed = 0.3f;
        buttonMoveDistance = 0.07f;
        initialButtonPosition = button.localPosition;
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isBeingPressed)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 4.0f))
            {

                if (hit.collider.tag == button.tag)
                {
                    isBeingPressed = true;
                    print(button.tag + " pressed");
                }
            }
        }

        if(isBeingPressed)
        {
            buttonPress();
        }
        else
        {
            buttonRelease();
        }

	}

    void buttonPress()
    {
        if ((button.localPosition.x - initialButtonPosition.x) < buttonMoveDistance)
        {
            button.Translate((Vector3.down * buttonSpeed) * Time.deltaTime);
        }
        else
        {
            isBeingPressed = false;
        }
      
    }

    void buttonRelease()
    {
        if ((button.localPosition.x - initialButtonPosition.x) > 0)
        {
            button.Translate((Vector3.up * buttonSpeed) * Time.deltaTime);
        }
    }
}
