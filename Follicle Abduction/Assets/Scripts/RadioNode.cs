using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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
		if(showingInputField && isServer) {
			
			InputField field = inputField.GetComponent<InputField> ();
			if (Input.GetKeyUp (KeyCode.Return)) {
				print ("hit enter");
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
			print ("TEXT FIELD: " + field.text);
		} else if (Input.GetKeyDown (KeyCode.R)) { //for debugging in human mode!
			PlayCorrectSong ();
		}
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

	public override void onStartAction() {
		if (state == NodeState.UNLOCKED) {
			if (choseCorrectSong) {
				PlayCorrectSong ();
			} else {
				PlayWrongSong ();
			}
		}
	}

	public override void Select() {
		isSelected = true;
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
		isSelected = false;
		if (outline) {
			print ("deselect");
			outline.SetActive (false);
			inputField.SetActive(false);
		}
	}

	public override void onEndAction() {
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
