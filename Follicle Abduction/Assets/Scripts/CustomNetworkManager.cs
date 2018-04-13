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
		Debug.Log("Host stopped.");
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

		// Handle connections while loading scene
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
				GameObject humanSpawn = GameObject.Find("HumanSpawn");
				if (!humanSpawn) {
					// Probably on the main menu
					Debug.Log("Couldn't find a human spawn - not spawning");
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
					Debug.Log("Couldn't find an alien spawn - not spawning");
					return;
				}
				Transform alienSpawnLocation = alienSpawn.GetComponent<Transform>();
				playerObject = Instantiate(alienPrefab, alienSpawnLocation);
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
		Debug.Log("Scene loaded: " + isLoadingScene);
		SpawnPlayerForConnection(conn, role, playerControllerId);
	}

	IEnumerator StopHostAfterSceneLoad() {
		yield return new WaitUntil(() => !isLoadingScene);
		StopHost();
	}

	public override void OnClientConnect(NetworkConnection conn){
		Debug.Log("Client connect!");
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
		Debug.Log("Got a message from client to change scenes");
		LoadSceneMessage msg = networkMessage.ReadMessage<LoadSceneMessage>();
		NetworkLoadScene(msg.newScene);
	}

	public void NetworkLoadScene(string sceneName) {
		if(isLoadingScene) {
			Debug.Log("Already loading scene");
			return;
		}

		if(isHost) {
			Debug.Log("We're the host and we're switching scenes");
			isLoadingScene = true;
			ServerChangeScene(sceneName);
		} else {
			isLoadingScene = true;
			Debug.Log("We want to change scenes but we're not the host!");
			LoadSceneMessage msg = new LoadSceneMessage();
			msg.newScene = sceneName;
			client.Send(LoadSceneMessage.type, msg);
		}
	}

	public override void OnServerSceneChanged(string sceneName) {
		Debug.Log("Server scene changed!");
		isLoadingScene = false;
	}

	// Called when we're the server and the client disconnects
	public override void OnServerDisconnect(NetworkConnection conn) {
		Debug.Log("OnServerDisconnect");
		if (conn.connectionId < maxConnectedPlayers) {
			// Kick everyone to the main menu if a player leaves
			NetworkLoadScene(mainMenuSceneName);
			StartCoroutine(StopHostAfterSceneLoad());
		}
	}

	// Called when we're the client and the server disconnects
	public override void OnClientDisconnect(NetworkConnection conn) {
		Debug.Log("OnClientDisconnect");
		StopClient();
		SceneManager.LoadScene(mainMenuSceneName);
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
		isLoadingScene = false;
		Debug.Log("OnClientSceneChanged");
		if (!ClientScene.ready) {
			ClientScene.Ready(conn);
		}

		bool addPlayer = doesMyPlayerNeedToSpawn();
		if (addPlayer)
		{
			Debug.Log("Attempting to add player with role: " + myRole);
			NotifyServerSpawnPlayer(conn, myRole);
		}
	}

	bool doesMyPlayerNeedToSpawn() {
		bool answer = false;
		if (ClientScene.localPlayers.Count == 0)
		{
			// no players exist
			Debug.Log("No players found");
			answer = true;
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
			answer = true;
		}

		return answer;
	}

	public void setMyRole(string role) {
		myRole = role;
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
			Debug.Log("Disconnecting extra player");
			conn.Disconnect();
		}
	}

	public bool isTheHost() // Used by LevelSelect.cs to determine role
	{
			return isHost;
	}
}