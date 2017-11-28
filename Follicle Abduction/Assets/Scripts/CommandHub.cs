using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

// Unity networking is stupid
public class CommandHub : NetworkBehaviour {

	private Dictionary<string, Action<string>> callbacks;

	void Start() {
		callbacks = new Dictionary<string, Action<string>>();
	}

	[Command]
	public void CmdSendCommand(string cmd) {
		if(isServer){
			ReceiveCommand(cmd);
		} else {
			Debug.Log("Sending command to server: " + cmd);
		}
	}

	void ReceiveCommand(string cmd) {
		callbacks[cmd](cmd);
	}

	public void RegisterCallback(string cmd, Action<string> callback){
		callbacks[cmd] = callback;
	}

	public void TestMethod() {
		Debug.Log("Hi");
	}
}
