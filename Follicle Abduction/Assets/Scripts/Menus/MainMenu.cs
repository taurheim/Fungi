using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
    Main game menu. Allows for settings, level selection, and debug mode to be toggled.
 */
public class MainMenu : MonoBehaviour
{

	public Button startGame;
	public Button quit;

	public Dropdown levelDropdown;
	public Dropdown roleDropdown;

	private string playerRole;

	public bool isDebug = false;

	// Use this for initialization
	void Start ()
	{
		if (startGame != null) {
			startGame.onClick.AddListener (selectedScene);
		}
		if (quit != null) {
			quit.onClick.AddListener (quitGame);
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void selectedScene ()
	{
		playerRole = roleDropdown.value.ToString ();

		if (levelDropdown.value == 0) {
			loadScene ("Level 1");
		} else if (levelDropdown.value == 1) {
			loadScene ("Level 2");
		} else if (levelDropdown.value == 2) {
			loadScene ("Level 3");
		}

	}

	public bool loadScene (string scene)
	{
		if (scene.Equals ("Level 1")) {
			Dictionary<string, string> levelParams = new Dictionary<string, string> () {
				{ "playerRole", getPlayerRole () },
				{ "isDebug", isDebug.ToString () }
			};
			LevelManager.Load ("Level_1", levelParams);
			return true;
		} else if (scene.Equals ("Level 2")) {
			Dictionary<string, string> levelParams = new Dictionary<string, string> () {
				{ "playerRole", getPlayerRole () },
				{ "isDebug", isDebug.ToString () }
			};
			LevelManager.Load ("Level_2", levelParams);
			return true;
		} else if (scene.Equals ("Level 3")) {
			Dictionary<string, string> levelParams = new Dictionary<string, string> () {
				{ "playerRole", getPlayerRole () },
				{ "isDebug", isDebug.ToString () }
			};
			LevelManager.Load ("Level_3", levelParams);
			return true;
		} else {
			return false;
		}

	}

	void quitGame ()
	{
		Application.Quit ();
	}


	public void updateSettings ()
	{

	}

	public bool settingsUpdatedSuccessfully ()
	{
		return false;
	}

	public void setPlayerRole (string role)
	{
		playerRole = role;
	}

	public string getPlayerRole ()
	{
		return playerRole;
	}

	public string getHighestAvailableLevel ()
	{
		return null;
	}

}
