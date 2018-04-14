using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
	Add this to an object that needs to have state synchronization of ANY KIND (other than the player)

	Currently only supports "interact" but can be expanded to more functionality.

	This script assumes that the 'localplayer' tag has been set
 */

[RequireComponent(typeof(NetworkIdentity))]
public class NetworkedObject : NetworkBehaviour {

	protected NetworkedPlayer localPlayer;

	protected CustomNetworkManager networkManager;

	public virtual void Start () {
		InitializeNetworkedObject();
	}

	protected void InitializeNetworkedObject() {
		networkManager = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();
		GameObject playerObject = GameObject.FindWithTag("networkmanager").GetComponent<CustomNetworkManager>().getPlayerObject();
		if(playerObject){
			localPlayer = playerObject.GetComponent<NetworkedPlayer>();
		}

		if(isServer) {
			RpcFixPosition(gameObject.transform.position);
		} else if (playerObject) {
			localPlayer.CmdUpdateMyPosition(this.gameObject);
		}
	}

	/*
		I HATE UNITY NETWORKING
	 */

	[ClientRpc]
	public void RpcFixPosition(Vector3 setTo) {
		if(gameObject.transform.position != setTo) {
			Debug.Log("Had to update position of " + gameObject.name);
			Debug.Log("From: " + gameObject.transform.position);
			Debug.Log("To: " + setTo);
		}
		gameObject.transform.position = setTo;
	}
	 /*
	 	END I HATE UNITY NETWORKING
	  */

	public void NetworkInteract() {
		GameObject playerObject = GameObject.FindWithTag("networkmanager").GetComponent<CustomNetworkManager>().getPlayerObject();
		if(playerObject){
			localPlayer = playerObject.GetComponent<NetworkedPlayer>();
		}
		if (isServer) {
			RpcInteract();
		} else {
			localPlayer.CmdInteractWith(this.gameObject);
		}
	}

	public void NetworkInteract(string str) {
		GameObject playerObject = GameObject.FindWithTag("networkmanager").GetComponent<CustomNetworkManager>().getPlayerObject();
		if(playerObject){
			localPlayer = playerObject.GetComponent<NetworkedPlayer>();
		}
		if (isServer) {
			RpcInteractStr(str);
		} else {
			localPlayer.CmdInteractWithStr(this.gameObject, str);
		}
	}

	protected virtual void Interact() {
		Debug.Log("Interact called - not implemented yet");
	}

	protected virtual void Interact(string str) {
		Debug.Log("Interact called - not implemented yet");
	}

	[ClientRpc]
	public virtual void RpcInteract() {
		Interact();
	}

	[ClientRpc]
	public virtual void RpcInteractStr(string str) {
		Interact(str);
	}
}
