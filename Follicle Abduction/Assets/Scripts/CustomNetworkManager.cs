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
	public GameMenu menu;

	public GameObject humanPrefab;
	public GameObject alienPrefab;

	private GameObject playerObject;
	private int playerCount;
	private bool isHost = false;

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

	// Called when someone joins the unity server
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		// Legacy code
		// TODO remove it (but leave this method overriden)
		if (conn.hostId == 0) {
			menu.NotifyPlayerAdded (conn, playerControllerId);
		}
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader) {
		Debug.Log("OnServerAddPlayer");
		RoleMessage msg = extraMessageReader.ReadMessage<RoleMessage>();
		string role = msg.role;

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

	public override void OnClientConnect(NetworkConnection conn){
		Debug.Log("Client connect!");
		ClientScene.Ready(client.connection);

		// This shouldn't be determined here
		string role = isHost ? "human" : "alien";

		RoleMessage msg = new RoleMessage();
		msg.role = role;
		ClientScene.AddPlayer(conn, 0, msg);
	}
}