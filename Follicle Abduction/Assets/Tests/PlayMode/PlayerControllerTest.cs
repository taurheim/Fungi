using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters;

public class PlayerControllerTest : MonoBehaviour {



	//NOTE: THESE TESTS ARE CURRENTLY EXPECTED TO FAIL! 
	//This is due to a currently incomplete movement model which can not be properly tested
	//using virtual inputs or mock loop inputs as they demand controller input.

	[UnityTest]
	public IEnumerator PlayerMovesXAxis() 
	{
		//uses virtual input to see whether the player moves on x axis and not y
		GameObject initalPos = new GameObject ();
		var playerA = new GameObject ().AddComponent<FirstPersonController> ();
		initalPos.transform.position = new Vector3 (0, 0);
		playerA.transform.position = initalPos.transform.position;

		CrossPlatformInputManager.SetAxis ("Horizontal", 1);

		yield return null;

		Assert.True(playerA.transform.position.x > initalPos.transform.position.x ||
			playerA.transform.position.x < initalPos.transform.position.x &&
			playerA.transform.position.z == initalPos.transform.position.z);


	}

	[UnityTest]
	public IEnumerator PlayerMovesZAxis() 
	{
		//uses virtual input to see whether the player moves on y axis and not x
		GameObject initalPos = new GameObject ();
		var playerA = new GameObject ().AddComponent<FirstPersonController> ();
		initalPos.transform.position = new Vector3 (0, 0);
		playerA.transform.position = initalPos.transform.position;

		yield return null;

		CrossPlatformInputManager.SetAxis ("Virtical", 1);

		Assert.True(playerA.transform.position.z > initalPos.transform.position.z ||
			playerA.transform.position.z < initalPos.transform.position.z &&
			playerA.transform.position.x == initalPos.transform.position.x);


	}

	[UnityTest]
	public IEnumerator PlayerMovesXAxisNoInput() 
	{
		//uses virtual input to see whether the player moves on x axis and not y
		GameObject initalPos = new GameObject ();
		var playerA = new GameObject ().AddComponent<FirstPersonController> ();
		initalPos.transform.position = new Vector3 (0, 0);
		playerA.transform.position = initalPos.transform.position;

		yield return null;

		Assert.True(playerA.transform.position.x > initalPos.transform.position.x ||
			playerA.transform.position.x < initalPos.transform.position.x &&
			playerA.transform.position.z == initalPos.transform.position.z);


	}

	[UnityTest]
	public IEnumerator PlayerMovesZAxisNoInput() 
	{
		//uses virtual input to see whether the player moves on y axis and not x
		GameObject initalPos = new GameObject ();
		var playerA = new GameObject ().AddComponent<FirstPersonController> ();
		initalPos.transform.position = new Vector3 (0, 0);
		playerA.transform.position = initalPos.transform.position;

		yield return null;


		Assert.True(playerA.transform.position.z > initalPos.transform.position.z ||
			playerA.transform.position.z < initalPos.transform.position.z &&
			playerA.transform.position.x == initalPos.transform.position.x);


	}

	[UnityTest]
	public IEnumerator PlayerPressesButtonToCompletePuzzle() 
	{
		yield return null;
		Assert.True (false);

	}

}
