using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneNode : Node {

	public AudioClip ring;

	private Patrol[] guards;
	private AudioSource source;
	private GameObject phone;
	private bool ringing;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		guards = Object.FindObjectsOfType<Patrol> ();
		ringing = false;
	}
	
	// Update is called once per frame
	void Update () {
		//temp while waiting for ray-picking to work
		if ((state == NodeState.UNLOCKED) && (Input.GetKeyUp (KeyCode.P))) {
			Ring ();
		} else if (ringing) {
			if (!source.isPlaying) {
				StopRinging ();
			}
		}
	}

	public override void HandleMouseDown() {
		print ("CLICKED PHONE");
		if (state == NodeState.UNLOCKED) {
			source.PlayOneShot (ring);
		}
	}

	public override void HandleMouseUp() {
		return;
	}

	void Ring() {
		if (!ringing) {
			source.PlayOneShot (ring);
			ringing = true;
			foreach (Patrol guard in guards) {
				GameObject parent = transform.parent.gameObject;
				print (guard);
				if (parent) {
					guard.AddSecondaryTarget (parent);
				}
			}
		}
	}

	void StopRinging () {
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
