using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public string sceneName;
	// private LevelManager levelManager

	// void Start() {
	// 	levelManager = GameObject.FindWithTag("levelmanager").GetComponent<LevelManager>();
	// }

	private void OnTriggerEnter(Collider other) {
		MonoBehaviour.print(other.gameObject.tag);
		if (other.gameObject.CompareTag("playerA")){
			SceneManager.LoadScene(sceneName);
		}
	}
}
