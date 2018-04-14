using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Node attached to the phone to allow the alien to control it. Implements Node.
	Alien can ring the phone by clicking it when it's unlocked, which distracts the guards
 */
public class PhoneNode : Node
{

	public AudioClip ring;
	public Patrol[] guards;
	private AudioSource source;
	private GameObject phone;
	private bool ringing;
    public bool continuous;

	// Use this for initialization
	public override void initializeNode ()
	{
		source = GetComponent<AudioSource> ();
		ringing = false;
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update();
		if (ringing && !source.isPlaying && !continuous) {
			NetworkInteract("stopRinging");
		}

		if (Input.GetKeyUp (KeyCode.P)) { //for debugging in human mode!
			onStartAction ();
		}
	}

	public override void onStartAction ()
	{
		if (state == NodeState.COMPLETED) {
			if (!ringing) {
				NetworkInteract("startRinging");
			} else {
				NetworkInteract("stopRinging");
			}
		}
	}

	public override void onEndAction ()
	{
		return;
	}

	protected override void Interact(string str) {
		if (str == "startRinging") {
			StartRinging();
		} else if (str == "stopRinging") {
			StopRinging();
		}
	}


	public void StartRinging ()
	{
		//If a sound is actually set...
		if (source && ring){
			source.PlayOneShot (ring);
		}
		//Otherwise silent "ring"? Can generalize this behaviour to non-phone objects later
		ringing = true;
		print ("ringing");
		if (guards != null) {
			foreach (Patrol guard in guards) {
				GameObject parent = transform.parent.gameObject;
				print (guard);
				if (parent) {
					guard.AddSecondaryTarget (parent);
				}
			}
		}
	}



	public void StopRinging ()
	{
		//If a sound is actually set...
		if (source && ring){
			source.Stop();
		}
		ringing = false;
		foreach (Patrol guard in guards) {
			GameObject parent = transform.parent.gameObject;
			print (parent);
			if (parent) {
				guard.RemoveSecondaryTarget (parent);
			}
		}
	}
}
