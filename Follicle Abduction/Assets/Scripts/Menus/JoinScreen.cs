using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkManager))]
public class JoinScreen : MonoBehaviour {

    [SerializeField] Button connect;
    [SerializeField] InputField port;
    [SerializeField] InputField ipAddress;
    [SerializeField] Button back;

    public MenuManager menuManager;

    public CustomNetworkManager manager;

    void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();
        connect.onClick.AddListener(connectButton);
        back.onClick.AddListener(backButton);
	}

    void Update()
    {
        if (manager.IsClientConnected())    
        {
            Debug.Log("Connected");
            onConnectedToHost();
        }
    }

    void connectButton()       // Try to connect to specified host
    {
        manager.networkAddress = ipAddress.text;
        manager.networkPort = int.Parse(port.text);
        manager.StartClient();
    }

    void backButton()
    {
        menuManager.hideJoinScreen();
        menuManager.displayStartScreen();

        manager.StopClient();
    }


    void onConnectedToHost()
    {
        menuManager.hideJoinScreen();
        menuManager.displayLevelSelect();
    }



    int portNum;
    public int getPort()
    {
        return portNum;
    }

    public void setPort(int num)
    {
        portNum = num;
    }

    string ipAddr;
    public string getIP()
    {
        return ipAddr;
    }

    public void setIP(string addr)
    {
        ipAddr = addr;
    }

}
