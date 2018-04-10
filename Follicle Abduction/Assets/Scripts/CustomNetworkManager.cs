using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoleMessage : MessageBase {
	public static short type = MsgType.Highest + 1;
	public bool isHost;
	public string role;
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
		Debug.Log("Scene loading: " + isLoadingScene);
		SpawnPlayerForConnection(conn, role, playerControllerId);
	}

	IEnumerator StopHostAfterSceneLoad() {
		yield return new WaitUntil(() => !isLoadingScene);
		StopHost();
	}

	public override void OnClientConnect(NetworkConnection conn){
		Debug.Log("Client connect!");
		ClientScene.Ready(client.connection);

		// This shouldn't be determined here
		myRole = isHost ? "alien" : "human";

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

	// Called when we're the server and the client disconnects
	public override void OnServerDisconnect(NetworkConnection conn) {
		Debug.Log("OnServerDisconnect");
		NetworkLoadScene(mainMenuSceneName);
		StartCoroutine(StopHostAfterSceneLoad());
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
			Debug.Log("Attempting to add player with role: " + myRole);
			NotifyServerSpawnPlayer(conn, myRole);
		}
	}

	public void setMyRole(string role) {
		myRole = role;
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