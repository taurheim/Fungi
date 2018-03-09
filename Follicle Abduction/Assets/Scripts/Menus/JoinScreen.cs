using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinScreen : MonoBehaviour {

    [SerializeField] Button connect;
    [SerializeField] InputField port;
    [SerializeField] InputField ipAddress;
    [SerializeField] Button back;

    public MenuManager menuManager;

    void Start ()
    {
        back.onClick.AddListener(backButton);
	}
	
    void connectButton()
    {

    }

    void portField()
    {

    }

    void ipField()
    {

    }

    void backButton()
    {
        menuManager.hideJoinScreen();
        menuManager.displayStartScreen();
    }


    void onConnectedToHost()
    {
        menuManager.hideJoinScreen();
        menuManager.displayLevelSelect();
    }

}
