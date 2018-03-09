using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

// Unit tests for HumanPlayer script

public class HumanPlayerTests {

	HumanPlayer player;

	[SetUp]
	public void SetUp(){
		GameObject obj = new GameObject ();
		obj.AddComponent<HumanPlayer>();
		player = obj.GetComponent<HumanPlayer>();
	}

	[Test]
	public void HumanPicksUpObject() {
		int i = player.pickups;
		GameObject pickup = new GameObject();
		pickup.AddComponent<CapsuleCollider>();
		pickup.tag = "pickup";
		pickup.SetActive(true);
		player.HandleTrigger(pickup.GetComponent<CapsuleCollider>());
		Assert.False(pickup.active);
		Assert.True(player.pickups == i+1);
	}

	[Test]
	public void HumanDoesNotPickUpObject() {
		int i = player.pickups;
		GameObject pickup = new GameObject();
		pickup.AddComponent<CapsuleCollider>();
		pickup.tag = "Untagged";
		pickup.SetActive(true);
		player.HandleTrigger(pickup.GetComponent<CapsuleCollider>());
		Assert.True(pickup.active);
		Assert.True(player.pickups == i);
	}

	[Test]
	public void HumanPositionReset() {
		Vector3 iPos = player.transform.position;
		player.transform.position = new Vector3(iPos.x+5, iPos.y+5, iPos.z+5);
		player.ResetPosition();
		Assert.True(iPos == player.transform.position);
	}

}
