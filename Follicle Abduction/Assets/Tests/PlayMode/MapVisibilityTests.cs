using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapVisibilityTests {

	//Tests for setVisibilityForTag()

	[UnityTest]
	public IEnumerator TurnsOffMapVisibilityOfObjectsWithGivenTag() {
		MapVisibility.setVisibilityForTag ("doToggle", false);

		yield return null;

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("doToggle")){
			foreach (Transform child in obj.transform) {
				if (child.CompareTag("mapIcon")) {
					Assert.False(child.GetComponent<Renderer>().enabled);
				}
			}
		}
	}
		
	[UnityTest]
	public IEnumerator TurnsOnMapVisibilityOfObjectsWithGivenTag() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("mapIcon")) {
			obj.GetComponent<Renderer> ().enabled = false;
		}
		MapVisibility.setVisibilityForTag ("doToggle", true);

		yield return null;

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("doToggle")){
			foreach (Transform child in obj.transform) {
				if (child.CompareTag("mapIcon")) {
					Assert.True(child.GetComponent<Renderer>().enabled);
				}
			}
		}
	}

	[UnityTest]
	public IEnumerator DoesNotTurnOffMapVisibilityOfObjectsWithoutGivenTag() {
		MapVisibility.setVisibilityForTag ("doToggle", false);
		yield return null;
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("dontToggle")){
			foreach (Transform child in obj.transform) {
				if (child.CompareTag("mapIcon")) {
					Assert.True(child.GetComponent<Renderer>().enabled);
				}
			}
		}
	}

	[UnityTest]
	public IEnumerator DoesNotTurnOnMapVisibilityOfObjectsWithoutGivenTag() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("mapIcon")) {
			obj.GetComponent<Renderer> ().enabled = false;
		}
		MapVisibility.setVisibilityForTag ("doToggle", true);

		yield return null;

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("dontToggle")){
			foreach (Transform child in obj.transform) {
				if (child.CompareTag("mapIcon")) {
					Assert.False(child.GetComponent<Renderer>().enabled);
				}
			}
		}
	}

	//Tests for isVisibleOnMap()

	[UnityTest]
	public IEnumerator ChecksThatObjectIsMapVisible() {
		Assert.True (MapVisibility.isVisibleOnMap (GameObject.FindGameObjectWithTag ("doToggle")));
		Assert.True (MapVisibility.isVisibleOnMap (GameObject.FindGameObjectWithTag ("dontToggle")));
		return null;
	}

	[UnityTest]
	public IEnumerator ChecksThatObjectIsNotMapVisible() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("mapIcon")) {
			obj.GetComponent<Renderer> ().enabled = false;
		}
		Assert.False(MapVisibility.isVisibleOnMap(GameObject.FindGameObjectWithTag("doToggle")));
		Assert.False(MapVisibility.isVisibleOnMap(GameObject.FindGameObjectWithTag("dontToggle")));
		return null;
	}

	//SetUp and TearDown

	[SetUp]
	public void SetUp() {
		//Map visibility is on by default
		MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Wall")).tag = "dontToggle";
		MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Wall")).tag = "dontToggle";
		MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Wall")).tag = "doToggle";
		MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Wall")).tag = "doToggle";
	}

	[TearDown]
	public void TearDown() {
		foreach (var obj in GameObject.FindObjectsOfType<GameObject> ()) {
			Object.Destroy(obj);
		}
	}
	
}
