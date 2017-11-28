using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{

    public Button startGame;
    public Button quit;

    public Dropdown levelDropdown;
    public Dropdown roleDropdown;

    private string playerRole;

    public bool isDebug = false;

    // Use this for initialization
    void Start()
    {
		if (startGame != null) {
			startGame.onClick.AddListener (selectedScene);
		}
		if (quit != null) {
			quit.onClick.AddListener (quitGame);
		}
    }

    // Update is called once per frame
    void Update()
    {

    }

    void selectedScene()
    {
        playerRole = roleDropdown.value.ToString();

        if (levelDropdown.value == 0)
        {
            loadScene("LevelC");
        }
        else if (levelDropdown.value == 1)
        {
            loadScene("Level1");
        }
        else if (levelDropdown.value == 2)
        {
            loadScene("LevelAA");
        }
        else if (levelDropdown.value == 3)
        {
            loadScene("LevelB");
        }

    }

    public bool loadScene(string scene)
    {
        if (scene.Equals("LevelC"))
        {
            Dictionary<string, string> levelParams = new Dictionary<string, string>() {
                { "playerRole", getPlayerRole()},
                { "isDebug", isDebug.ToString()}
            };
            LevelManager.Load("LevelC", levelParams);
            return true;
        }
        else if (scene.Equals("Level1"))
        {
            LevelManager.Load("Level1");
            return true;
        }
        else if (scene.Equals("LevelAA"))
        {
            Dictionary<string, string> levelParams = new Dictionary<string, string>() {
                { "playerRole", getPlayerRole()},
                { "isDebug", isDebug.ToString()}
            };
            LevelManager.Load("LevelAA", levelParams);
            return true;
        }
        else if (scene.Equals("LevelB")) {
            Dictionary<string, string> levelParams = new Dictionary<string, string>() {
                { "playerRole", getPlayerRole()},
                { "isDebug", isDebug.ToString()}
            };
            LevelManager.Load("LevelB", levelParams);
            return true;
        }

        else
        {
            return false;
        }

    }

    void quitGame()
    {
        Application.Quit();
    }


    public void updateSettings()
    {

    }

    public bool settingsUpdatedSuccessfully()
    {
        return false;
    }

    public void setPlayerRole(string role)
    {
        playerRole = role;
    }

    public string getPlayerRole()
    {
        return playerRole;
    }

    public string getHighestAvailableLevel()
    {
        return null;
    }

}
