using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Handles level loading. Allows for parameters to be passed in while loading a level
    From: https://forum.unity.com/threads/unity-beginner-loadlevel-with-arguments.180925/
 */

public static class LevelManager
{

	public static string[] levels = { "level_1", "level_2", "level_3", "level_4"};
 
	private static Dictionary<string, string> parameters;

	private static int currentLevel = 0;

	private static int maxLevel = 4;

	public static string getCurrentLevel() {
		return levels[currentLevel];
	}

	public static void goToNextLevel() {
		currentLevel++;
	}

	public static string[] getLevelList() {
		return levels;
	}

	public static void resetState() {
		currentLevel = 0;
	}

	public static bool isLevelAvailable(string levelName) {
		for(int i = 0; i <= currentLevel; i++) {
			if (levels[i] == levelName) {
				return true;
			}
		}
		return false;
	}

	public static bool isGameComplete() {
		return currentLevel == maxLevel;
	}

	public static void Load (string sceneName, Dictionary<string, string> parameters = null)
	{
		LevelManager.parameters = parameters;
		SceneManager.LoadScene (sceneName);
	}

	public static void Load (string sceneName, string paramKey, string paramValue)
	{
		LevelManager.parameters = new Dictionary<string, string> ();
		LevelManager.parameters.Add (paramKey, paramValue);
		SceneManager.LoadScene (sceneName);
	}

	public static Dictionary<string, string> getSceneParameters ()
	{
		return parameters;
	}

	public static string getParam (string paramKey)
	{
		if (parameters == null)
			return "";
		if (!parameters.ContainsKey (paramKey)) {
			Debug.Log ("Couldn't find " + paramKey + " in LevelManager.parameters");
			Debug.Log ("Existing keys are: ");
			foreach (var key in parameters.Keys) {
				Debug.Log (key);
			}
			return "";
		}
		return parameters [paramKey];
	}

	public static void setParam (string paramKey, string paramValue)
	{
		if (parameters == null)
			LevelManager.parameters = new Dictionary<string, string> ();
		LevelManager.parameters.Add (paramKey, paramValue);
	}
 
}