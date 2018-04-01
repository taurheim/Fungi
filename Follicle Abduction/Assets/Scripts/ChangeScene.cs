using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 
	Component to make a trigger change scenes.
	Used to change levels when reaching the goal.
 */

public class ChangeScene : MonoBehaviour {

	public string sceneName;
	private bool changingScenes; // Prevent double calling
	
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("playerA") && !changingScenes){
			changingScenes = true;
			Debug.Log("Collision detected - switching scenes!");
			GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>().NetworkLoadScene(sceneName);
		}
	}
}
