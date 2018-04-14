using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
	Add this to an object that needs to have state synchronization of ANY KIND (other than the player)

	Currently only supports "interact" but can be expanded to more functionality.

	This script assumes that the 'localplayer' tag has been set
 */
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
	}

	public void NetworkInteract() {
		if (isServer) {
			RpcInteract();
		} else {
			localPlayer.CmdInteractWith(this.gameObject);
		}
	}

	public void NetworkInteract(string str) {
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
