using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour {

    [SerializeField] Button host;
    [SerializeField] Button join;
    [SerializeField] Button settings;
    [SerializeField] Button quit;

    public MenuManager menuManager;


	// Use this for initialization
	void Start ()
    {
        join.onClick.AddListener(joinButton);
        host.onClick.AddListener(hostButton);
        settings.onClick.AddListener(settingsButton);
        quit.onClick.AddListener(quitButton);
    }

    void hostButton()
    {
        menuManager.hideStartScreen();
        menuManager.displayHostScreen();
    }

    void joinButton()
    {
        menuManager.hideStartScreen();
        menuManager.displayJoinScreen();
    }

    void settingsButton()
    {

    }

    void quitButton()
    {
        Application.Quit();
    }
}
