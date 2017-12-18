using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	State manager for the console (alien) view. For now remembers which node is currently selected
 */
public class ConsoleManager : MonoBehaviour {

	private Node selectedNode;
	public TextMesh nodeData;

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
