using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioNode : Node {

	public string correctSongName;
	public AudioClip correctSong;
	public AudioClip wrongSong;
	public Patrol[] songLovingGuards;
	public GameObject inputField;

	private AudioSource source;
	private GameObject radio;
	private bool playing;
	private bool choseCorrectSong;
	private bool showingInputField;


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		playing = false;
		choseCorrectSong = false;
		InputField field = inputField.GetComponent<InputField>();
		field.interactable = true;
		showingInputField = false;
	}
	
	// Update is called once per frame
	void Update () {
		//temp while waiting for ray-picking to work
		if (playing && !source.isPlaying) {
			StopPlayingCorrectSong ();
		}
		if(showingInputField) {
			InputField field = inputField.GetComponent<InputField> ();
			if (Input.GetKeyUp (KeyCode.Return)) {
				print ("hit enter");
				if (field.text == correctSongName) {
					print ("correct song!");
					choseCorrectSong = true;
				}
			} else if (Input.GetKeyDown (KeyCode.Backspace)) {
				print("backspace key");
				field.text = field.text.Remove (field.text.Length - 1);
			} else {
				field.text += Input.inputString;
			}
		} else if (Input.GetKeyDown (KeyCode.R)) { //for debugging in human mode!
			PlayCorrectSong ();
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

	void PlayWrongSong() {
		source.PlayOneShot (wrongSong);
	}

	void PlayCorrectSong() {
		if (!playing) {
			source.Stop ();
			source.PlayOneShot (correctSong);
			playing = true;
			foreach (Patrol guard in songLovingGuards) {
				GameObject parent = transform.parent.gameObject;;
				if (parent) {
					print ("adding secondary target");
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
