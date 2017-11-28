using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioNode : Node {

	public string correctSongName;
	public AudioClip correctSong;
	public AudioClip wrongSong;
	public Patrol[] songLovingGuards;
	private GameObject inputField;

	private AudioSource source;
	private GameObject radio;
	private bool playing;
	private bool choseCorrectSong;


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		playing = false;
		choseCorrectSong = false;
		inputField = GameObject.Find("InputField");
		print (inputField);
	}
	
	// Update is called once per frame
	void Update () {
		//temp while waiting for ray-picking to work
		if (playing && !source.isPlaying) {
			StopPlayingCorrectSong ();
		}
	}

	public override void StartAction() {
		if (state == NodeState.UNLOCKED) {
			if (choseCorrectSong) {
				PlayCorrectSong ();
			} else {
				PlayWrongSong ();
			}
		}
	}

	public override void Select() {
		selected = true;
		if (outline) {
			print ("select");
			outline.SetActive (true);
			inputField.SetActive (true);
		}
	}

	public override void Deselect() {
		selected = false;
		if (outline) {
			print ("deselect");
			outline.SetActive (false);
			inputField.SetActive(false);
		}
	}

	public override void EndAction() {
		return;
	}

	void PlayWrongSong() {
		source.PlayOneShot (wrongSong);
	}

	void PlayCorrectSong() {
		if (!playing) {
			source.PlayOneShot (correctSong);
			playing = true;
			foreach (Patrol guard in songLovingGuards) {
				GameObject parent = transform.parent.gameObject;
				print (guard);
				if (parent) {
					guard.AddSecondaryTarget (parent);
				}
			}
		}
	}

	void StopPlayingCorrectSong () {
		if (playing) {
			playing = false;
			foreach (Patrol guard in songLovingGuards) {
				GameObject parent = transform.parent.gameObject;
				print (parent);
				if (parent) {
					guard.RemoveSecondaryTarget (parent);
				}
			}
		}
	}
}
