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

	public string[] songNames;
	public AudioClip[] songs;
	public int correctSongIndex;
	public Patrol[] songLovingGuards; // These guards get distracted (move towards the radio) when the correct song is played

	private TextMesh songTextDisplay;
	private AudioSource source;
	private GameObject radio;
	private bool playing;
	private bool choseCorrectSong;
	private int currentSong;


	public override void initializeNode () {

		source = GetComponent<AudioSource> ();
		songTextDisplay = transform.Find("SongName").GetComponent<TextMesh>();
		playing = false;
		choseCorrectSong = false;
	}
	
	protected override void Update () {
		base.Update();
		// Playing music
		if (playing && !source.isPlaying) {
			StopPlayingCorrectSong ();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow)) {

		}
	}

	public void ChooseSong(bool correct) {
		//NetworkInteract
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

	public override void onEndAction() {
		return;
	}

	public void PlayWrongSong() {
		//source.PlayOneShot (wrongSong);
	}

	public void PlayCorrectSong() {
		if (!playing) {
			// Actually play the song!
			// if (source && correctSong){
			// 	source.Stop ();
			// 	source.PlayOneShot (correctSong);
			// }
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
