using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Manager for the door opening/door closing logic
 */
public class DoorLogic : NetworkedObject
{

	private bool isLocked;
	// Unused atm, maybe unnecessary
	public bool isOpen;

	public GameObject door;

	public GameObject button;

	public override void Start ()
	{
		base.Start();
		isLocked = false;
		this.isOpen = false;

	}

	void Update ()
	{
		if (button.GetComponent<ButtonPress>().getButtonStatus() && !this.isOpen) {
			button.GetComponent<ButtonPress>().stopPress();
			NetworkInteract();
		} else if (button.GetComponent<ButtonPress>().getButtonStatus() && this.isOpen) {
			button.GetComponent<ButtonPress>().stopPress ();
			NetworkInteract();
		}
	}

	// Called every frame if door is opening, slowly opens door every frame
	public void open ()
	{
		if (!isLocked) {
			door.GetComponent<DoorAnimation>().open ();
			this.isOpen = true;
			this.GetComponent<BoxCollider>().size = new Vector3 (0, 0, 0);

            AudioSource sfx = GetComponent<AudioSource>();
            sfx.Play();
		}
	}

	// Called every frame if door is closing, slowly closes door every frame
	public void close ()
	{
		door.GetComponent<DoorAnimation>().close ();
		this.isOpen = false;
		this.GetComponent<BoxCollider>().size = new Vector3 (5, 5, 1);
	}

    protected override void Interact(){
    	if (!isOpen){
    		this.open();
    	}
    	else{
    		this.close();
    	}
    }



}
