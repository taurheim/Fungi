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

	// Use this for initialization
	void Start ()
	{
		source = GetComponent<AudioSource> ();
		ringing = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ringing && !source.isPlaying) {
			StopRinging ();
		}
		if (Input.GetKeyUp (KeyCode.P)) { //for debugging in human mode!
			StartAction ();
		}
	}

	public override void StartAction ()
	{
		if (state == NodeState.UNLOCKED) {
			Ring ();
		}
	}

	public override void EndAction ()
	{
		return;
	}

	public void Ring ()
	{
		if (!ringing) {
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
	}

	public void StopRinging ()
	{
		if (ringing) {
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
}
