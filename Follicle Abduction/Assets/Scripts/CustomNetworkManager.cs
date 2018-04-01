using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/*
    Custom implementation of the NetworkManager interface. Currently used mainly as a debug tool.
 */
public class CustomNetworkManager : NetworkManager
{
	public class RoleMessage : MessageBase {
		public string role;
	}
	public GameObject humanPrefab;
	public GameObject alienPrefab;

	private GameObject playerObject;
	private int playerCount;
	private bool isHost = false;
	public string myRole = "";
	private bool isLoadingScene = false;

    public bool clientConnected = false;    //Used by HostScreen.cs to determine when a client connects

    // This is called by the "host" - could be either alien or human player
    // Passes itself (the player object) in
    public void registerPlayerObject(GameObject obj) {
		playerObject = obj;
	}

	// This should probably only be used by networking.
	public GameObject getPlayerObject() {
		return playerObject;
	}

	// TODO which player you start as shouldn't be decided here
	// Instead, we should have some other way of telling what the player chose so we can
	// spawn the right player in OnServerAddPlayer()
	public override void OnStartHost() {
		isHost = true;
	}

	public override void OnStopHost() {
		isHost = false;
		myRole = "";
	}

	// Called when someone joins the unity server
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
        // Do nothing just incase something else calls this
    }

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		RoleMessage msg = extraMessageReader.ReadMessage<RoleMessage>();
		string role = msg.role;
		Debug.Log("OnServerAddPlayer: " + role);

		if (isLoadingScene) {
			StartCoroutine(WaitForSceneLoad(conn, role, playerControllerId));
		} else {
			SpawnPlayerForConnection(conn, role, playerControllerId);
		}
	}

	void SpawnPlayerForConnection(NetworkConnection conn, string role, short playerControllerId) {
		GameObject playerObject;
		switch (role) {
			case "human":
				// Spawn human
				Transform humanSpawn = GameObject.Find("HumanSpawn").GetComponent<Transform>();
				playerObject = Instantiate(humanPrefab, humanSpawn);
				break;
			case "alien":
				// Spawn alien
				Transform alienSpawn = GameObject.Find("AlienSpawn").GetComponent<Transform>();
				playerObject = Instantiate(alienPrefab, alienSpawn);
				break;
			default:
				Debug.Log("Invalid role: " + role);
				return;
		}

		NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);
	}

	IEnumerator WaitForSceneLoad(NetworkConnection conn, string role, short playerControllerId) {
		Debug.Log("Waiting for scene to finish loading...");
		yield return new WaitUntil(() => !isLoadingScene);
		Debug.Log("Scene loading: " + isLoadingScene);
		SpawnPlayerForConnection(conn, role, playerControllerId);
	}

	public override void OnClientConnect(NetworkConnection conn){
		Debug.Log("Client connect!");
		ClientScene.Ready(client.connection);

		// This shouldn't be determined here
		myRole = isHost ? "human" : "alien";

		NotifyServerSpawnPlayer(conn, myRole);
	}

	public void NetworkLoadScene(string sceneName) {
		if(isHost) {
			Debug.Log("We're the host and we're switching scenes");
			isLoadingScene = true;
			ServerChangeScene(sceneName);
		}
	}

	public override void OnServerSceneChanged(string sceneName) {
		Debug.Log("Server scene changed!");
		isLoadingScene = false;
	}

	private void NotifyServerSpawnPlayer(NetworkConnection conn, string playerRole) {
		RoleMessage msg = new RoleMessage();
		msg.role = playerRole;
		ClientScene.AddPlayer(conn, 0, msg);
	}

	/*
		Hack: Assume that setting client to not ready means that we need to spawn them
	*/
	public override void OnClientSceneChanged(NetworkConnection conn) {
		Debug.Log("OnClientSceneChanged");
		if (!ClientScene.ready) {
			ClientScene.Ready(conn);
		}

		bool addPlayer = false;
		if (ClientScene.localPlayers.Count == 0)
		{
			// no players exist
			Debug.Log("No players found");
			addPlayer = true;
		}

		bool foundPlayer = false;
		foreach (var playerController in ClientScene.localPlayers)
		{
			if (playerController.gameObject != null)
			{
				Debug.Log("Found a player object");
				foundPlayer = true;
				break;
			}
		}

		if (!foundPlayer)
		{
			// there are players, but their game objects have all been deleted
			Debug.Log("There are players but their game objects have been deleted");
			addPlayer = true;
		}
		if (addPlayer)
		{
			NotifyServerSpawnPlayer(conn, myRole);
		}
	}

    // Called when someone joins the server
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (conn.connectionId == 1) //Check that a client has joined (and not the host player)
        { 
            clientConnected = true;
        }
    }

    public bool isTheHost() // Used by LevelSelect.cs to determine role
    {
        return isHost;
    }
}