using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/* 
	Component for radio node behaviour.
	Handles song input for Alien player and playing song clips.
	Also handles distracting the correct guards given the correct song.
 */

public class RadioNode : Node {

	public string correctSongName;
	public AudioClip correctSong;
	public AudioClip wrongSong;
	public Patrol[] songLovingGuards; // These guards get distracted (move towards the radio) when the correct song is played
	public GameObject inputField;

	private AudioSource source;
	private GameObject radio;
	private bool playing;
	private bool choseCorrectSong;
	private bool showingInputField;


	void Start () {
		source = GetComponent<AudioSource> ();
		playing = false;
		choseCorrectSong = false;
		InputField field = inputField.GetComponent<InputField>();
		field.interactable = true;
		showingInputField = false;
	}
	
	void Update () {
		if (playing && !source.isPlaying) { // Temp until ray casting is implemented
			StopPlayingCorrectSong ();
		}
		if(showingInputField && isServer) {
			HandleKeyInput();
		} else if (Input.GetKeyDown (KeyCode.R)) { // For debugging!
			PlayCorrectSong ();
		}
	}

	void HandleKeyInput() {
		InputField field = inputField.GetComponent<InputField> ();
		if (Input.GetKeyUp (KeyCode.Return)) {
			if (field.text == correctSongName) {
				print ("correct song!");
				ChooseSong (true);
			} else {
				ChooseSong (false);
			}
		} else if (Input.GetKeyDown (KeyCode.Backspace)) {
			print("backspace key");
			if (field.text.Length > 0) {
				field.text = field.text.Remove (field.text.Length - 1);
			}
		} else {
			field.text += Input.inputString;
		}
		print ("Text field: " + field.text);
	}

	public void ChooseSong(bool correct) {
		if(isServer) {
			RpcChooseSong(correct);
		} else {
			CmdChooseSong(correct);
		}
	}

	[ClientRpc]
	public void RpcChooseSong(bool correct) {
		choseCorrectSong = correct;
	}

	[Command]
	public void CmdChooseSong(bool correct) {
		RpcChooseSong (correct);
	}

	// This node's action is to play a song
	public override void StartAction() {
		if (state == NodeState.UNLOCKED) {
			if (choseCorrectSong) {
				PlayCorrectSong ();
			} else {
				PlayWrongSong ();
			}
		}
	}

	// Highlights and opens input field when node is selected
	public override void Select() {
		selected = true;
		if (outline) {
			print ("select");
			outline.SetActive (true);
			inputField.SetActive (true);
			InputField field = inputField.GetComponent<InputField>();
			field.ActivateInputField ();
			field.interactable = true;
			showingInputField = true;
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

	public void PlayWrongSong() {
		source.PlayOneShot (wrongSong);
	}

	public void PlayCorrectSong() {
		if (!playing) {
			if (source && correctSong){
				source.Stop ();
				source.PlayOneShot (correctSong);
			}
			playing = true;
			if (songLovingGuards != null) {
				// Attract the guards to the radio!
				foreach (Patrol guard in songLovingGuards) {
					GameObject parent = transform.parent.gameObject;;
					if (parent) {
						print ("adding secondary target");
						guard.AddSecondaryTarget (parent);
					}
				}
			}
		}
	}

	public void StopPlayingCorrectSong () {
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
