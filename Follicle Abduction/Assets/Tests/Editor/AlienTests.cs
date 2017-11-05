using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AlienTests {

	Alien alien;

	[SetUp]
	public void SetUp() {
		//Map visibility is on by default
		alien = new Alien ();
	}

	//Tests for suspiciousness levels
	//Initial suspiciousness defaults to 0

	[Test]
	public void DoesSomethingSuspicious() {
		int initialSuspiciousness = alien.getSuspiciousness ();
		alien.doSomethingSuspicious (10);
		int finalSuspiciousness = alien.getSuspiciousness ();
		Assert.True ((finalSuspiciousness - initialSuspiciousness) == 10);
	}

	[Test]
	public void GetsCapturedAtMaxSuspiciousness() {
		alien.doSomethingSuspicious (alien.maxSuspiciousness);
		Assert.True (alien.hasBeenCaptured());
	}

	[Test]
	public void DoesNotGetCapturedAtLessThanMaxSuspiciousness() {
		alien.doSomethingSuspicious (alien.maxSuspiciousness - 1);
		Assert.False (alien.hasBeenCaptured());
	}

	[Test]
	public void GetsCapturedAtMoreThanMaxSuspiciousness() {
		alien.doSomethingSuspicious (alien.maxSuspiciousness + 1);
		Assert.True (alien.hasBeenCaptured());
	}
		
}
