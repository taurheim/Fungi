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

	//MUST SET NODE DATA to SongSelector prefab!!!!

	public string[] songNames;
	public AudioClip[] songs;
	public int correctSongIndex;
	public Patrol[] songLovingGuards; // These guards get distracted (move towards the radio) when the correct song is played
	
	private AudioSource audioSource;
	private TextMesh songTextDisplay;
	private GameObject radio;
	private bool playing;
	private int currentSelectedSong;
	private int currentPlayingSong;


	public override void initializeNode () {
		playing = false;
		currentSelectedSong = -1;
		currentPlayingSong = -1;
		songTextDisplay = nodeData.transform.Find("SongName").GetComponent<TextMesh>();
		audioSource = GetComponent<AudioSource>();
	}
	
	protected override void Update () {
		base.Update();
		// Playing music
		if (playing && !audioSource.isPlaying) {
			NetworkInteract("stopSong");
		}
		if (isSelected && (state == NodeState.COMPLETED)) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) {
				NetworkInteract("incrementSongIndex");
			} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
				NetworkInteract("decrementSongIndex");
			}
		}
	}

	public override void onStartAction() {
		if ((state == NodeState.COMPLETED) && (currentSelectedSong >= 0)) {
			if (playing) {
				if (currentPlayingSong == currentSelectedSong) {
					NetworkInteract("stopSong");
				} else {
					NetworkInteract("switchSong");
				}
			} else {
				NetworkInteract("startSong");
			}
		}
	}

	protected override void Interact(string str) {
		switch (str) {
			case "incrementSongIndex":
				IncrementSongIndex();
				break;
			case "decrementSongIndex":
				DecrementSongIndex();
				break;
			case "startSong":
				StartSong();
				break;
			case "switchSong":
				SwitchSong();
				break;
			case "stopSong":
				StopSong();
				break;
		}
	}

	public void IncrementSongIndex(){
		currentSelectedSong = (currentSelectedSong + 1) % songs.Length;
		UpdateSongText();
	}

	public void DecrementSongIndex(){
		currentSelectedSong -= 1;
		Debug.Log("decrement, now song index is " + currentSelectedSong);
		if (currentSelectedSong < 0) {
			currentSelectedSong = songs.Length - 1;
			Debug.Log("out of bounds so we changed it to " + currentSelectedSong);
		}
		UpdateSongText();
	}

	private void UpdateSongText() {
		songTextDisplay.text = songNames[currentSelectedSong];
	}

	public void StartSong() {
		playing = true;
		currentPlayingSong = currentSelectedSong;
		audioSource.PlayOneShot(songs[currentPlayingSong]);
		if ((currentPlayingSong == correctSongIndex) && (songLovingGuards != null)) {
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

	public void SwitchSong() {
		StopSong();
		StartSong();
	}

	public void StopSong () {
		playing = false;
		audioSource.Stop();
		if (currentPlayingSong == correctSongIndex){
			foreach (Patrol guard in songLovingGuards) {
				GameObject parent = transform.parent.gameObject;
				if (parent) {
					guard.RemoveSecondaryTarget (parent);
				}
			}
		}
	}
}
