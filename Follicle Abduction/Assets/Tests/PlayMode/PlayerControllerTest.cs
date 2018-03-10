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
    GameObject human;

    [SetUp]
    public void SetUp()
    {

        //Load test scene
        Resources.Load("Prefabs/floor");
        human = (GameObject)Resources.Load("FPSController");



    }

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            GameObject.DestroyObject(obj);
        }
    }


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

		Assert.True(playerA.transform.position.x >= initalPos.transform.position.x ||
			playerA.transform.position.x <= initalPos.transform.position.x &&
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


		Assert.True(playerA.transform.position.z >= initalPos.transform.position.z ||
			playerA.transform.position.z <= initalPos.transform.position.z &&
			playerA.transform.position.x == initalPos.transform.position.x);


	}

	[UnityTest]
	public IEnumerator PlayerInteractWorld() 
	{
        // checks to see if player is facing the button
        GameObject button = new GameObject();
        button.transform.position = new Vector3(0, 0);
        button.transform.rotation = new Quaternion(-1, 0, 0, 0);
        human.transform.position = new Vector3(button.transform.position.x + 1, button.transform.position.y);
        human.transform.rotation = new Quaternion(1, 0, 0, 0);


        yield return null;
		Assert.True (human.transform.rotation.x == -button.transform.rotation.x);
        Assert.True(human.transform.rotation.y == -button.transform.rotation.y);

	}

}
