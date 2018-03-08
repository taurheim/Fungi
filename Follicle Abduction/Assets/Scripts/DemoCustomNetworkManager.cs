using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
	Used in the demo networking scene. Implementation of NetworkManager that auto-assigns client authority to all users
	who join.
 */
public class DemoCustomNetworkManager : NetworkManager
{

	public Vector3 spawnLocation;
	
	public GameObject humanPrefab;
	public GameObject alienPrefab;

	private int playerCount = 0;
	private bool isHost = false;

	private GameObject playerObject;

	public void registerPlayerObject(GameObject obj) {
		playerObject = obj;
	}

	public GameObject getPlayerObject() {
		return playerObject;
	}

	public override void OnStartHost() {
		isHost = true;
	}

	public class RoleMessage : MessageBase {
		public int role; // 1 or 2
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		// Do nothing - this shouldn't be called by anything
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader reader) {
		RoleMessage msg = reader.ReadMessage<RoleMessage>();
		int role = msg.role;

		switch(role) {
			case 1:
				// Spawn human
				GameObject player = Instantiate(humanPrefab);
				NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
				break;
			case 2: 
				// Spawn alien
				GameObject alien = Instantiate(alienPrefab);
				NetworkServer.AddPlayerForConnection(conn, alien, playerControllerId);
				break;
			default:
				Debug.Log("Invalid role: " + role);
				break;
		}
	}

	public override void OnClientConnect(NetworkConnection conn) {
		ClientScene.Ready(client.connection);

		// This would normally be decided in another way
		int role = isHost ? 2 : 1;

		RoleMessage msg = new RoleMessage();
		msg.role = role;
		ClientScene.AddPlayer(conn, 0, msg);
	}
}
