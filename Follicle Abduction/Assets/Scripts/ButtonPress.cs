using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public Transform button;

	[SerializeField]
    Camera playerCamera;

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

        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 10, Color.green);

        if (Input.GetKeyDown(KeyCode.E) && !isBeingPressed)
        {
            int layerMask = 1;  //Rays only hit objects on default layer

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 5.0f, layerMask))
            {
                if (hit.collider.tag == button.tag)
                {
                    isBeingPressed = true;
                    Debug.Log(button.tag + " pressed");
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

    public bool getButtonStatus() {
        return isBeingPressed;
    }
}
