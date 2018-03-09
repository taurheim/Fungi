using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

//Only doing unit testing for non-networked methods, and non-UI based methods

public class RadioNodeTests {

	GameObject radioObj;
	GameObject radioNodeObj;
	GameObject guardObj;
	RadioNode radio;
	Patrol guard;
	Patrol[] guards;

	[SetUp]
	public void SetUp () {

		radioObj = new GameObject();
		radioNodeObj = new GameObject();
		radioNodeObj.transform.parent = radioObj.transform;
		radioNodeObj.AddComponent<AudioSource>();
		radio = radioNodeObj.AddComponent<RadioNode>();
		radio.correctSong = new AudioClip();
		guardObj = new GameObject();
		guard = guardObj.AddComponent<Patrol>();
		guards = new Patrol[1];
		guards[0] = guard;
	}

	[Test]
	public void MusicAttractsGuard() {
		radio.songLovingGuards = guards;
		radio.PlayCorrectSong();
		Assert.True(guard.secondaryTargets.Contains(radioObj));
	}

	[Test]
	public void MusicDoesNotAttractGuard() {
		radio.PlayCorrectSong();
		Assert.False(guard.secondaryTargets.Contains(radioObj));
	}

	[Test]
	public void MusicStopsNoLongerAttractsGuard() {
		radio.songLovingGuards = guards;
		radio.PlayCorrectSong();
		Assert.True(guard.secondaryTargets.Contains(radioObj));
		radio.StopPlayingCorrectSong();
		Assert.False(guard.secondaryTargets.Contains(radioObj));
	}
}
