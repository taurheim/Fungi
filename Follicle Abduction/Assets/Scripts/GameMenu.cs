using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
    Custom game menu. Handles networking & connections.
    This screen shows up after a level is selected, and acts as a lobby while the other player connects.
 */
public class GameMenu : NetworkBehaviour
{
    // TODO these cameras could be referenced in a better way
    public Camera waitingCamera;

    public Camera alienCam1, alienCam2;

    public CustomNetworkingHUD netManagerHUD;

    bool gamePaused = false;

    bool isDebug;

    [SyncVar]
    int connections = 0;

    public Vector3 spawnLocation;

    public GameObject playerPrefab;

    // Use this for initialization
    void Start()
    {
        isDebug = LevelManager.getParam("isDebug") == "True";
    }

    // While we're in the connection menu
    void Update()
    {
        if(!netManagerHUD.showGUI) return;

        if(isServer){
			if (NetworkServer.connections.Count != connections) 
			{
				connections = NetworkServer.connections.Count;		//Track how many connections there are
			}
        }

		// TODO better param setting and loading so we don't have to only compare strings?
		// on the other hand maybe this is the cleanest way to do this
        string role = LevelManager.getParam("playerRole");

        if(isDebug) {
			Debug.Log("We're in debug mode - don't wait for more connections");
            hideMenu();
            GameObject player = (GameObject)Instantiate (playerPrefab, spawnLocation, Quaternion.identity);
            if(role == "1"){
                player.GetComponentInChildren<Camera>().enabled = false;
            } else {
                alienCam1.enabled = false;
                alienCam2.enabled = false;
            }
            if(waitingCamera) {
                waitingCamera.enabled = false;
            }
        } else if(connections == 2){
            hideMenu();
        }
    }

    void hideMenu() {
        netManagerHUD.showGUI = false;
    }

    public void pauseGame()
    {
        //TODO Pausing the game for real
        gamePaused = true;
    }

    public bool isGamePaused()
    {
        return gamePaused;
    }

    public void NotifyPlayerAdded(NetworkConnection connection, short playerControllerId) {
        Debug.Log("Player has connected!");
        GameObject player = (GameObject)Instantiate (playerPrefab, spawnLocation, Quaternion.identity);
        if(isServer){
            player.GetComponentInChildren<Camera>().enabled = false;
        } else {
            alienCam1.enabled = false;
            alienCam2.enabled = false;
        }
        NetworkServer.AddPlayerForConnection (connection, player, playerControllerId);
        Debug.Log("Two players connected! Start game now!");
        if(isServer){
            Debug.Log("I'm the alien!");
        } else {
            Debug.Log("I'm the human!");
        }
        hideMenu();
    }
}