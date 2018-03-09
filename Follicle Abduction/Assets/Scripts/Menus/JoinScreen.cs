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
        connect.onClick.AddListener(connectButton);
        back.onClick.AddListener(backButton);
	}
	
    void connectButton()
    {

    }

    public void portField()
    {

    }

    public void ipField()
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
