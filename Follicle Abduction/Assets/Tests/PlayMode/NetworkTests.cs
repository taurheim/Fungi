using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.Networking;

public class NetworkTests : MonoBehaviour
{

    public NetworkManager SetupNetworkManager()
    {
        GameObject obj = new GameObject();
        NetworkManager netManager = obj.AddComponent<NetworkManager>();
        return netManager;
    }

    [Test]
    public void SetNetworkPort()
    {
        NetworkManager netManager = SetupNetworkManager();

        int port = 7777;

        netManager.networkPort = port;

        Assert.True(netManager.networkPort == port);   
    }

    [Test]
    public void SetNetworkAddress()
    {
        NetworkManager netManager = SetupNetworkManager();

        string address = "localhost";

        netManager.networkAddress = address;

        Assert.True(netManager.networkAddress == address);
    }

    [Test]
    public void SeeNumberOfPlayers()
    {
        NetworkManager netManager = SetupNetworkManager();

        int numPlayers = netManager.numPlayers;

        Assert.True(netManager.numPlayers == numPlayers);
    }

    [Test]
    public void NetworkManagerHUD()
    {
        GameObject obj = new GameObject();
        NetworkManager netManager = obj.AddComponent<NetworkManager>();
        NetworkManagerHUD netManagerHUD = obj.AddComponent<NetworkManagerHUD>();

        Assert.True(netManagerHUD.enabled == true);
    }

}
