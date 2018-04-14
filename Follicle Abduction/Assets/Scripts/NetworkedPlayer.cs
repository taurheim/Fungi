using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

/*
	This should be added to any object that can be a player.

	Because unity is stupid, we route all our networked calls through this player object. In an effort to minimize the code we have to throw into the player script, this is an extra script that will handle events
 */
public class NetworkedPlayer : NetworkBehaviour {

	public override void OnStartLocalPlayer() {
		GameObject.FindWithTag("networkmanager").GetComponent<CustomNetworkManager>().registerPlayerObject(this.gameObject);

		FirstPersonController controller = GetComponentInChildren<FirstPersonController>();
		if(controller) {
			controller.enabled = true;
			// If our local player is the human, disable AlienPlayer script as well.
			foreach(AlienPlayer alien in Object.FindObjectsOfTypeAll(typeof(AlienPlayer))) {
				alien.enabled = false;
			}
		}
		GetComponentInChildren<Camera>().enabled = true;
	}

	[Command]
	public void CmdInteractWith(GameObject obj) {
		obj.GetComponent<NetworkedObject>().RpcInteract();
	}

	[Command]
	public void CmdInteractWithStr(GameObject obj, string str) {
		obj.GetComponent<NetworkedObject>().RpcInteractStr(str);
	}
	
}
