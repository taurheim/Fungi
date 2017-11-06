using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinConditionTest : MonoBehaviour {


	GameObject human;

	[SetUp]
	public void SetUp() {

		//Load test scene
		Resources.Load ("Prefabs/floor");
		human = (GameObject)Resources.Load ("FPSController");



	}

	[TearDown]
	public void TearDown() {
		foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
			GameObject.DestroyObject (obj);
		}
	}

	[UnityTest]
	public IEnumerator MovePlayerAwayFromStaticWinAreaAndCheckWinCondition() {

		//"Move" character to away from static win area and check win condition
		GameObject winArea = new GameObject ();
		winArea.transform.position = new Vector3 (20, 9);
		human.transform.position = new Vector3(winArea.transform.position.x+1, winArea.transform.position.y+1);
		yield return null;
		Assert.True (winArea.transform.position != human.transform.position);

	}

	[UnityTest]
	public IEnumerator MovePlayerToStaticWinAreaAndCheckWinCondition() {

		//"Move" character to static win area and check win condition
		GameObject winArea = new GameObject ();
		winArea.transform.position = new Vector3 (20, 9);
		human.transform.position = new Vector3(20, 9);
		yield return null;
		Assert.True (winArea.transform.position == human.transform.position);

	}
		
	[UnityTest]
	public IEnumerator FindNewWinArea() {

		//Find new win area in scene
		GameObject winArea = GameObject.Find("Win Area");


		yield return null;
		Assert.True (winArea.transform != null);


	}

	[UnityTest]
	public IEnumerator MovePlayerAwayFromDynamicWinAreaAndCheckWinCondition() {

		//"Move" character to away from dynamic win area and check win condition
		GameObject winArea = GameObject.Find("Win Area");
		human.transform.position = new Vector3(0, 0);
		yield return null;
		Assert.True (winArea.transform.position != human.transform.position);


	}
	[UnityTest]
	public IEnumerator MovePlayerToDynamicWinAreaAndCheckWinCondition() {

		//"Move" character to dynamic win area and check win condition
		GameObject winArea = GameObject.Find("Win Area");
		human.transform.position = winArea.transform.position;
		yield return null;
		Assert.True (winArea.transform.position == human.transform.position);


	}
	[UnityTest]
	public IEnumerator WinMessageIsDisplayed() {

		//"Move" character to dynamic win area and check win condition
		GameObject winArea = GameObject.Find("Win Area");
		GameObject gameController = GameObject.Find ("Game Controller");
		human.transform.position = winArea.transform.position;
		yield return null;
		Assert.True (gameController == true);


	}


}
