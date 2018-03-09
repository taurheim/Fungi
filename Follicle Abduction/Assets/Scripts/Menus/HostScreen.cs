using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HostScreen : MonoBehaviour {

    [SerializeField] Button back;
    [SerializeField] Text message;      //TODO: Make this blink

    public MenuManager menuManager;

    void Start ()
    {
        back.onClick.AddListener(backButton);
	}
	
	void backButton()
    {
        menuManager.hideHostScreen();
        menuManager.displayStartScreen();
    }

    void onClientConnected()
    {
        menuManager.hideHostScreen();
        menuManager.displayLevelSelect();
    }
}
