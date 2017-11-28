using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DemoCustomNetworkManager : NetworkManager {

	public Vector3 spawnLocation;

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		if(conn.hostId == 0){
			GameObject player = (GameObject) Instantiate (playerPrefab, spawnLocation, Quaternion.identity);
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
			PlayerAuthority[] playerControlledObjects = FindObjectsOfType<PlayerAuthority>();
			foreach(PlayerAuthority auth in playerControlledObjects) {
				auth.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
			}
		}
	}
}
