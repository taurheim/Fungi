using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
 
public class CustomNetworkManager : NetworkManager {

    public GameMenu menu;

    private bool isDebug;

    void Start() {
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (conn.hostId == 0) {
            menu.NotifyPlayerAdded(conn, playerControllerId);
        }
    }
 /* 
    public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
    {
        Debug.Log("OnServerAddPlayer");
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
 
 
 
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log("OnClientSceneChanged");
        ClientScene.AddPlayer(conn, 0);
    }
 
    public override void OnClientConnect(NetworkConnection conn)
    {
        //base.OnClientConnect(conn);
    }
 */
 
}