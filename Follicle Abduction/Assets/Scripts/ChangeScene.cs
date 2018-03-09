using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public string sceneName;
	
	// Prevent double calling
	private bool changingScenes;
	// private LevelManager levelManager

	// void Start() {
	// 	levelManager = GameObject.FindWithTag("levelmanager").GetComponent<LevelManager>();
	// }

	private void OnTriggerEnter(Collider other) {
		if (!changingScenes && other.gameObject.CompareTag("playerA")){
			changingScenes = true;
			Debug.Log("Collision detected - switching scenes!");
			GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>().NetworkLoadScene(sceneName);
		}
	}
}
