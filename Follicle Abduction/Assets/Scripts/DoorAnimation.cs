using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Used to animate doors based on state
 */
public class DoorAnimation : MonoBehaviour
{

	Animation animations;

	// If true, will open the door when the scene loads
	[SerializeField]
	bool startOpen = false;

	bool isOpen = false;

	void Start ()
	{
		animations = GetComponent<Animation> ();

		// This is just incase the door need to be open at the start
		if (startOpen) {
			instantOpen ();
		}
	}

	public void open ()
	{
		if (!isOpen) {
			animations.Play ("open", PlayMode.StopAll);
			isOpen = true;
		}
	}

	public void close ()
	{
		if (isOpen) {
			animations.Play ("close", PlayMode.StopAll);
			isOpen = false;
		}
	}

	void instantOpen ()
	{
		animations.Play ("instantOpen", PlayMode.StopAll);
		isOpen = true;
	}
}
