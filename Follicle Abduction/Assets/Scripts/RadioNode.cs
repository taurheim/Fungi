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
	public GameObject songSelector;

	private AudioSource source;
	private GameObject radio;
	private bool playing;
	private bool choseCorrectSong;
	private bool showingSongSelector;


	public override void initializeNode () {
		source = GetComponent<AudioSource> ();
		playing = false;
		choseCorrectSong = false;
		songSelector.SetActive(false);
		Dropdown dropDown = songSelector.GetComponentInChildren<Dropdown>();
		dropDown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(dropDown);
            });
		dropDown.interactable = true;
		showingSongSelector = false;
	}
	
	protected override void Update () {
		base.Update();
		// Playing music
		if (playing && !source.isPlaying) { // Temp until ray casting is implemented
			StopPlayingCorrectSong ();
		}
		// Showing text input
		if (isSelected && !showingSongSelector && isServer && (state == NodeState.COMPLETED)) {
			ShowSongSelector();
		} else if (!isSelected && showingSongSelector) {
			Debug.Log("hide!!!");
			HideSongSelector();
		}
	}

	private void DropdownValueChanged(Dropdown dropDown) {
		if (dropDown.options[dropDown.value].text == correctSongName) {
			ChooseSong(true);
		} else {
			ChooseSong(false);
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
		if (state == NodeState.COMPLETED) {
			if (choseCorrectSong) {
				PlayCorrectSong ();
			} else {
				PlayWrongSong ();
			}
		}
	}

	private void ShowSongSelector() {
		songSelector.SetActive (true);
		Dropdown dropDown = songSelector.GetComponentInChildren<Dropdown>();
		dropDown.interactable = true;
		showingSongSelector = true;
	}

	private void HideSongSelector() {
		songSelector.SetActive(false);
		Dropdown dropDown = songSelector.GetComponentInChildren<Dropdown>();
		dropDown.interactable = false;
		showingSongSelector = false;
	}

	public override void onEndAction() {
		return;
	}

	public void PlayWrongSong() {
		//source.PlayOneShot (wrongSong);
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
