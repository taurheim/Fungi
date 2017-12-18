using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Manager for the door opening/door closing logic
 */
public class DoorLogic : MonoBehaviour {

    private bool isLocked; // Unused atm, maybe unnecessary
    private bool isOpen;

    public GameObject door;

    public GameObject button;

	void Start () {
        isLocked = false;
        this.isOpen = false;

    }
	
	void Update () {
        if (button.GetComponent<ButtonPress>().getButtonStatus() && !this.isOpen) {
            button.GetComponent<ButtonPress>().stopPress();
            this.open();
        }
        
        else if (button.GetComponent<ButtonPress>().getButtonStatus() && this.isOpen) {
            button.GetComponent<ButtonPress>().stopPress();
            this.close();
        }
        
    }

    // Called every frame if door is opening, slowly opens door every frame
    public void open() {
        if (!isLocked) {
            door.GetComponent<DoorAnimation>().open();
            this.isOpen = true;
            this.GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);
        }
    }

    // Called every frame if door is closing, slowly closes door every frame
    public void close() {
        door.GetComponent<DoorAnimation>().close();
        this.isOpen = false;
        this.GetComponent<BoxCollider>().size = new Vector3(5, 5, 1);
    }
}
