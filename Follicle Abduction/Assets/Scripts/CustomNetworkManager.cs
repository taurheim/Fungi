using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoleMessage : MessageBase {
	public static short type = MsgType.Highest + 1;
	public bool isHost;
	public string role;
}

public class LoadSceneMessage : MessageBase {
	public static short type = MsgType.Highest + 5;
	public string newScene;
}

/*
    Custom implementation of the NetworkManager interface. Currently used mainly as a debug tool.
 */
public class CustomNetworkManager : NetworkManager
{
	public GameObject humanPrefab;
	public GameObject alienPrefab;

	private string mainMenuSceneName = "MainMenu";

	private GameObject playerObject;
	private int playerCount;
	public bool isHost = false;
	public string myRole = "";
	private bool isLoadingScene = false;
	private int maxConnectedPlayers = 2;

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

	private void Start() {
		NetworkServer.RegisterHandler(LoadSceneMessage.type, HandleLoadSceneMessage);
	}

	/*
		Used for testing levels
	 */
	public void debugAsRole(string role) {
		myRole = role;
		StartHost();
	}

	public override void OnStartHost() {
		isHost = true;
	}

	public override void OnStopHost() {
		isHost = false;
		clientConnected = false;
		myRole = "";
		Debug.Log("[Server] Stopped hosting server.");
	}

	// Called when someone joins the unity server
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		// Do nothing just incase something else calls this
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		RoleMessage msg = extraMessageReader.ReadMessage<RoleMessage>();
		string role = msg.role;
		Debug.Log("[Server] Got a message to add a player: " + role + "(Connection: "+ conn.connectionId + ", controller: " + playerControllerId + ")");
		StartCoroutine(WaitForSceneLoad(conn, role, playerControllerId));
	}

	void SpawnPlayerForConnection(NetworkConnection conn, string role, short playerControllerId) {
		if (role == "") {
			Debug.Log("[Server] Got a request to spawn without a role. Rejecting: Client #" + conn.connectionId + " (controller: " + playerControllerId + ")");
			return;
		}

		GameObject playerObject;
		switch (role) {
			case "human":
				// Spawn human
				GameObject humanSpawn = GameObject.Find("HumanSpawn");
				if (!humanSpawn) {
					// Probably on the main menu
					Debug.Log("[Server] Couldn't find a human spawn - not spawning");
					return;
				}
				Transform humanSpawnLocation = humanSpawn.GetComponent<Transform>();
				playerObject = Instantiate(humanPrefab, humanSpawnLocation);
				break;
			case "alien":
				// Spawn alien
				GameObject alienSpawn = GameObject.Find("AlienSpawn");
				if (!alienSpawn) {
					// Probably on the main menu
					Debug.Log("[Server] Couldn't find an alien spawn - not spawning");
					return;
				}
				Transform alienSpawnLocation = alienSpawn.GetComponent<Transform>();
				playerObject = Instantiate(alienPrefab, alienSpawnLocation);
				break;
			default:
				Debug.Log("[Server] Invalid role: " + role + " for Client #" + conn.connectionId + "(controller: " + playerControllerId + ")");
				return;
		}

		NetworkServer.AddPlayerForConnection(conn, playerObject, playerControllerId);
	}

	IEnumerator WaitForSceneLoad(NetworkConnection conn, string role, short playerControllerId) {
		Debug.Log("[Server] Waiting for scene to finish loading. Then spawning " + role + " for Client " + conn.connectionId);
		yield return new WaitUntil(() => !isLoadingScene);
		Debug.Log("[Server] Scene finished loading. Now spawning player " + role + " for connection " + conn.connectionId);
		SpawnPlayerForConnection(conn, role, playerControllerId);
	}

	IEnumerator StopHostAfterSceneLoad() {
		yield return new WaitUntil(() => !isLoadingScene);
		StopHost();
	}

	public override void OnClientConnect(NetworkConnection conn){
		Debug.Log("[Client] Connected to server. Setting ready then spawning if role set");
		ClientScene.Ready(client.connection);

		if(myRole != "") {
			NotifyServerSpawnPlayer(conn, myRole);
		} else {
			Debug.Log("No role selected. Not spawning.");
		}
	}

	/*
		This will get called on the server if the client wants to change scenes.
	 */
	private void HandleLoadSceneMessage(NetworkMessage networkMessage) {
		Debug.Log("[Server] Got a message from client to change scenes");
		LoadSceneMessage msg = networkMessage.ReadMessage<LoadSceneMessage>();
		NetworkLoadScene(msg.newScene);
	}

	public void NetworkLoadScene(string sceneName) {
		if(isLoadingScene) {
			Debug.Log("[Server] Tried to load scene, but we're already loading it");
			return;
		}

		if(isHost) {
			Debug.Log("[Server] Switching scenes");
			isLoadingScene = true;
			
			// Preload the scene
			Debug.Log("[Server] Preloading scene.");
			SceneManager.LoadScene(sceneName);

			// Load the scene on all connected clients
			Debug.Log("[Server] Network loading scene.");
			ServerChangeScene(sceneName);
		} else {
			isLoadingScene = true;
			Debug.Log("[Client] Sending message to host to change scenes");
			LoadSceneMessage msg = new LoadSceneMessage();
			msg.newScene = sceneName;
			client.Send(LoadSceneMessage.type, msg);
		}
	}

	public override void OnServerSceneChanged(string sceneName) {
		Debug.Log("[Server] Scene changed!");
		isLoadingScene = false;
	}

	// Called when we're the server and the client disconnects
	public override void OnServerDisconnect(NetworkConnection conn) {
		Debug.Log("[Server] Client disconnected");
		if (conn.connectionId < maxConnectedPlayers) {
			// Kick everyone to the main menu if a player leaves
			NetworkLoadScene(mainMenuSceneName);
			StartCoroutine(StopHostAfterSceneLoad());
		}
	}

	// Called when we're the client and the server disconnects
	public override void OnClientDisconnect(NetworkConnection conn) {
		Debug.Log("[Client] Disconnected from server.");
		StopClient();
		SceneManager.LoadScene(mainMenuSceneName);
	}

	private void NotifyServerSpawnPlayer(NetworkConnection conn, string playerRole) {
		Debug.Log("[Client] We're notifying the server that we should spawn " + playerRole);
		RoleMessage msg = new RoleMessage();
		msg.role = playerRole;
		ClientScene.AddPlayer(conn, 0, msg);
	}

	/*
		Hack: Assume that setting client to not ready means that we need to spawn them
	*/
	public override void OnClientSceneChanged(NetworkConnection conn) {
		isLoadingScene = false;
		Debug.Log("[Client] Our scene changed! Spawn us!");
		if (!ClientScene.ready) {
			ClientScene.Ready(conn);
		}
		NotifyServerSpawnPlayer(conn, myRole);
	}

	// Called when someone joins the server
	public override void OnServerConnect(NetworkConnection conn)
	{
		if(conn.connectionId < maxConnectedPlayers) {
			// A player joined (host player will have id 0)
			if (conn.connectionId != 0) 
			{
				clientConnected = true;
			}
		} else if (conn.connectionId >= maxConnectedPlayers) {
			// Disconnect this player, we have too many
			Debug.Log("[Server] Disconnecting extra player");
			conn.Disconnect();
		}
	}

	public bool isTheHost() // Used by LevelSelect.cs to determine role
	{
			return isHost;
	}
}