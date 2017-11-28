using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTestSphere : NetworkBehaviour {

	public Material mat1,mat2;

	[SyncVar]
	int currentMat = 1;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(currentMat == 1){
			GetComponent<MeshRenderer>().material = mat1;
		} else {
			GetComponent<MeshRenderer>().material = mat2;
		}
	}

	void OnMouseDown() {
		if(isServer){
			RpcSwapMaterials();
		} else {
			CmdSwapMaterials();
		}
	}

	void SwapMaterials() {
		if(currentMat == 1){
			currentMat = 2;
		} else {
			currentMat = 1;
		}
	}

	[Command]
	void CmdSwapMaterials() {
		Debug.Log("Running command");
		RpcSwapMaterials();
	}

	[ClientRpc]
	void RpcSwapMaterials() {
		Debug.Log("Running RPC");
		SwapMaterials();
	}
}
