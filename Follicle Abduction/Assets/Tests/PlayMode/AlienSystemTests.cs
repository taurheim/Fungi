using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class AlienSystemTests {

	GameObject human;
	GameObject alien;
	GameObject guard;
	GameObject cameraNode;

	[SetUp]
	public void SetUp() {

		//Load test scene
		Resources.Load ("Prefabs/floor");
		human = (GameObject)Resources.Load ("FPSController");
		alien = (GameObject)Resources.Load ("GIC View");
		guard = (GameObject)Resources.Load ("Guard");
		cameraNode = (GameObject)Resources.Load ("Camera Node");

	}

	[TearDown]
	public void TearDown() {
		foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>()) {
			GameObject.DestroyObject (obj);
		}
	}

	[UnityTest]
	public IEnumerator MapRendersSynchornizedPlayerMotion() {

		//Get all the stuff we need
		Camera mapCam = alien.GetComponentInChildren<Camera> ();
		MapObject humanMapObj = human.GetComponent<MapObject> ();
		GameObject icon = humanMapObj.mapIcon;
		humanMapObj.setVisibility (true);

		//Check that icon is visible in map
		Assert.True (ObjectIsVisibleByCamera (mapCam,icon));

		//"Move" character and check that icon position follows
		for (int i = 0; i < 10; i++) {
			human.transform.position = new Vector3( human.transform.position.x+1, human.transform.position.y);
			yield return null;
			Assert.True (human.transform.position == icon.transform.position);
		}
			
	}

	[UnityTest]
	public IEnumerator MapRendersSynchornizedGuardMotion() {

		//Get all the stuff we need
		Camera mapCam = alien.GetComponentInChildren<Camera> ();
		MapObject guardMapObj = guard.GetComponent<MapObject> ();
		GameObject icon = guardMapObj.mapIcon;
		guardMapObj.setVisibility (true);

		//Check that icon is visible in map
		Assert.True (ObjectIsVisibleByCamera (mapCam,icon));

		//Move guard (cannot move automatically without navMesh) and check position against icon
		for (int i = 0; i < 10; i++) {
			guard.transform.position = new Vector3( guard.transform.position.x+1, guard.transform.position.y);
			yield return null;
			Assert.True (guard.transform.position == icon.transform.position);
		}
	}

	[UnityTest]
	public IEnumerator UnlockingNodeDisplaysNewMapIcons() {

		//Get all the stuff we need
		Camera mapCam = alien.GetComponentInChildren<Camera> ();
		MapObject guardMapObj = guard.GetComponent<MapObject> ();
		GameObject icon = guardMapObj.mapIcon;
		Node node = cameraNode.GetComponent<Node> ();
		node.progressBar = null;

		//Link up guard to camera: is hidden until cam is completed
		node.tagForHiddenObjects = "securityCam1";
		guard.tag = "securityCam1";
		guardMapObj.setVisibility (false);
		Assert.False (ObjectIsVisibleByCamera (mapCam, icon));

		//Unlock & complete node
		node.unlockNode();
		node.completeNode ();
		yield return null;

		//Guard should now be visible
		Assert.True (ObjectIsVisibleByCamera (mapCam, icon));
	}

	[UnityTest]
	public IEnumerator BeingTooSuspiciousRestartsLevel() {
		//We don't have functionality for this yet
		Assert.True(false);
		yield return null;
	}

	bool ObjectIsVisibleByCamera(Camera cam, GameObject obj) {
		Renderer rend = obj.GetComponent<Renderer>();
		if (rend != null) {
			if (rend.enabled) {
				Vector3 viewPos = cam.WorldToViewportPoint (obj.transform.position);
				if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z >= 0) {
				//if ((cam.cullingMask & obj.layer) >0 ) {
					return true;
				}
			}
		}
		return false;
	}


}

