using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour {

    [SerializeField] Button host;
    [SerializeField] Button join;
    [SerializeField] Button quit;

    public MenuManager menuManager;

	// Use this for initialization
	void Start ()
    {
        join.onClick.AddListener(joinButton);
        host.onClick.AddListener(hostButton);
        quit.onClick.AddListener(quitButton);
    }

    public void hostButton()
    {
        menuManager.hideStartScreen();
        menuManager.displayHostScreen();
    }

    public void joinButton()
    {
        menuManager.hideStartScreen();
        menuManager.displayJoinScreen();
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
