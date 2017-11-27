using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneNode : MonoBehaviour {

	public AudioClip ring;

	private Patrol[] guards;
	private AudioSource source;
	private Node node;
	private GameObject phone;
	private bool ringing;

	// Use this for initialization
	void Start () {
		node = GetComponent<Node> ();
		source = GetComponent<AudioSource> ();
		guards = Object.FindObjectsOfType<Patrol> ();
		ringing = false;
	}
	
	// Update is called once per frame
	void Update () {
		//temp while waiting for ray-picking to work
		if ((node.state == NodeState.UNLOCKED) && (Input.GetKeyUp (KeyCode.P))) {
			Ring ();
		} else if (ringing) {
			if (!source.isPlaying) {
				StopRinging ();
			}
		}
	}

	void OnMouseDown() {
		if (node.state == NodeState.UNLOCKED) {
			source.PlayOneShot (ring);
		}
	}

	void Ring() {
		source.PlayOneShot (ring);
		ringing = true;
		foreach (Patrol guard in guards) {
			guard.AddSecondaryTarget (transform.parent.gameObject);
		}
	}

	void StopRinging () {
		ringing = false;
		foreach (Patrol guard in guards) {
			guard.RemoveSecondaryTarget (transform.parent.gameObject);
		}
	}
}
