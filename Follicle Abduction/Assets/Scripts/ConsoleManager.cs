using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleManager : MonoBehaviour {

	private Node selectedNode;
	public TextMesh nodeData;


	// Use this for initialization
	void Start () {
		selectedNode = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Select(Node node) {
		if (selectedNode) {
			selectedNode.Deselect ();
		}
		node.Select ();
		selectedNode = node;
		print (selectedNode);
		SetNodeData ();
	}

	void SetNodeData () {
		if (nodeData) {
			nodeData.text = "";
			foreach (string data in selectedNode.data) {
				nodeData.text += "\n";
				nodeData.text += data;
			}
		}
	}
}
