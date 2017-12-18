using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
	Used in the networking test scene. Allow both players to modify the material of the sphere.
 */
public class NetworkTestSphere : NetworkBehaviour {

	public Material mat1,mat2;

	int currentMat = 1;

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
