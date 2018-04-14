using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PhoneNodeTests {

	GameObject phoneObj;
	GameObject phoneNodeObj;
	GameObject guardObj;
	PhoneNode phone;
	Patrol guard;
	Patrol[] guards;

	[SetUp]
	public void SetUp () {

		phoneObj = new GameObject();
		phoneNodeObj = new GameObject();
		phoneNodeObj.transform.parent = phoneObj.transform;
		phoneNodeObj.AddComponent<AudioSource>();
		phone = phoneNodeObj.AddComponent<PhoneNode>();
		phone.ring = new AudioClip();
		guardObj = new GameObject();
		guard = guardObj.AddComponent<Patrol>();
		guards = new Patrol[1];
		guards[0] = guard;
	}

	[Test]
	public void PhoneRingAttractsGuard() {
		phone.guards = guards;
		phone.StartRinging();
		Assert.True(guard.secondaryTargets.Contains(phoneObj));
	}

	[Test]
	public void PhoneRingDoesNotAttractGuard() {
		phone.StartRinging();
		Assert.False(guard.secondaryTargets.Contains(phoneObj));
	}

	[Test]
	public void PhoneStopsRingingNoLongerAttractsGuard() {
		phone.guards = guards;
		phone.StartRinging();
		Assert.True(guard.secondaryTargets.Contains(phoneObj));
		phone.StopRinging();
		Assert.False(guard.secondaryTargets.Contains(phoneObj));
	}

}
