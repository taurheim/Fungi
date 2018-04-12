using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach a door to this node: its "action" will open and close it!

public class DoorNode : Node {

	public GameObject door;

	// Use this for initialization
	public override void initializeNode ()
	{
		// Initialization code
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update();
	}

	public override void onStartAction ()
	{
		if (state == NodeState.COMPLETED) {
			DoorLogic doorScript = door.GetComponent<DoorLogic>();
			doorScript.NetworkInteract();
		}
	}

}
