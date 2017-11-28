using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandSender : MonoBehaviour {
	GameObject player;

	void Update() {
		if(player == null){
			player = GameObject.FindGameObjectWithTag("playerA");
			if(player != null){
				GameObject.FindObjectOfType<CommandHub>().RegisterCallback("swap_materials", SendCommand);
			}
		}
	}

	public void SendCommand(string cmd) {
		player.GetComponent<CommandHub>().CmdSendCommand(cmd);
	}
}
