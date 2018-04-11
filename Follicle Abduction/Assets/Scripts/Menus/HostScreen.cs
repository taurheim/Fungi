using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkManager))]
public class HostScreen : MonoBehaviour {

    [SerializeField] Button back;
    [SerializeField] Text message;      //TODO: Make this text blink

    public MenuManager menuManager;

    public CustomNetworkManager manager;

    void Start ()
    {

        manager = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();
        back.onClick.AddListener(backButton);

        manager.StartHost();            // Start listening for clients
    }

    void Update()
    {
        if(manager.clientConnected)  
                                  
        {
            onClientConnected();
        }

    }
	
	public void backButton()
    {
        manager.StopHost();

        menuManager.hideHostScreen();
        menuManager.displayStartScreen();
    }

    public void onClientConnected()
    {
        menuManager.hideHostScreen();
        menuManager.displayLevelSelect();
    }
}
