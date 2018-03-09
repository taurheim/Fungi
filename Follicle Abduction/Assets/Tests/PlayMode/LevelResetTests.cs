using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;


//If nothing else goes in here can rename to "HumanFallsOffStageTest", but could also
//put other scenarios where the level is reset in here (such as guard capture).
//Would put it in HumanPlayerTests but it has to be a PlayTest to call Update
public class LevelResetTests {

	HumanPlayer player;

	[SetUp]
	public void SetUp(){
		GameObject obj = new GameObject ();
		obj.AddComponent<HumanPlayer>();
		player = obj.GetComponent<HumanPlayer>();
	}

	[UnityTest]
	public IEnumerator HumanPositionResetWhenFallsOffStage() {
		Vector3 iPos = player.transform.position;
		player.transform.position = new Vector3(iPos.x, -100, iPos.y);
		yield return null;
		Assert.True(iPos == player.transform.position);
	}
}
