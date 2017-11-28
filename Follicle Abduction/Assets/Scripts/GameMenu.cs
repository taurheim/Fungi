using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMenu : NetworkBehaviour
{

    public string playerA_tag, playerB_tag;

    public GameObject waitingCamera;
    // I'm going to hate myself
    public Camera alienCam1, alienCam2;

    public CustomNetworkingHUD netManagerHUD;

    bool gamePaused = false;

    bool isDebug;

    [SyncVar]
    int connections = 0;

    bool setupComplete = false;

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
        if(setupComplete) return;

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
            setupGameForRole(role);
            GameObject player = (GameObject)Instantiate (playerPrefab, spawnLocation, Quaternion.identity);
            if(role == "1"){
                player.GetComponentInChildren<Camera>().enabled = false;
            } else {
                alienCam1.enabled = false;
                alienCam2.enabled = false;
            }
        } else if(connections == 2){
            hideMenu();
        }
    }

    void hideMenu() {
        setupComplete = true;
        netManagerHUD.showGUI = false;
    }

    void setupGameForRole(string role){
        setupComplete = true;
        /* 
//        GameObject playerAObject = GameObject.FindGameObjectWithTag(playerA_tag);
//        GameObject playerBObject = GameObject.FindGameObjectWithTag(playerB_tag);

//        playerAObject.GetComponent<Camera>().enabled = role == "0";
//        playerBObject.GetComponent<Camera>().enabled = role == "1";
return;
        // TODO I hate this
        foreach(Behaviour behaviour in playerAObject.GetComponent<Components>().GetComponentList()) {
            behaviour.enabled = role == "0";
        }

        foreach(Behaviour behaviour in playerBObject.GetComponent<Components>().GetComponentList()) {
            if(behaviour) {
                behaviour.enabled = role == "1";
            }
        }
        setupComplete = true;*/
    }

    public void pauseGame()
    {
        //implement ability to pause game
    }

    public bool isGamePaused()
    {
        return gamePaused;
    }

    // I'm shit
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
            setupGameForRole("1");
        } else {
            Debug.Log("I'm the human!");
            setupGameForRole("0");
        }
    }

    void SpawnPlayer() {
        
    }
}