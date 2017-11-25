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
            loadScene("Demo_level");
        }
        else if (levelDropdown.value == 1)
        {
            loadScene("Level1");
        }
        else if (levelDropdown.value == 2)
        {
            loadScene("LevelA");
        }
        else if (levelDropdown.value == 3)
        {
            //load level 3
        }
    }

    public bool loadScene(string scene)
    {
        if (scene.Equals("Demo_level"))
        {
            Dictionary<string, string> levelParams = new Dictionary<string, string>() {
                { "playerRole", getPlayerRole()},
                { "isDebug", "true"}
            };
            LevelManager.Load("Demo_level", levelParams);
            return true;
        }
        else if (scene.Equals("Level1"))
        {
            LevelManager.Load("Level1");
            return true;
        }
        else if (scene.Equals("LevelA"))
        {
            Dictionary<string, string> levelParams = new Dictionary<string, string>() {
                { "playerRole", getPlayerRole()},
                { "isDebug", "true"}
            };
            LevelManager.Load("LevelA", levelParams);
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
