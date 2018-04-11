using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	State manager for the console (alien) view. For now remembers which node is currently selected
	TODO: Merge this with Alien.cs?
 */
public class ConsoleManager : MonoBehaviour
{

	private Node selectedNode;
	public TextMesh nodeData;

	/* 
	Make this node the currently selected node
	Selected means the node is highlighted and any extra info is displayed in the console data panel
	*/
	public void Select (Node node)
	{
		if (selectedNode) {
			selectedNode.Deselect ();
		}
		node.Select ();
		selectedNode = node;
		print (selectedNode);
		SetNodeData ();
	}

	/*
	Sets the console data to show the node data
	The data contains hints for the alien to help solve puzzles!
	*/
	void SetNodeData ()
	{
		// if (nodeData) {
		// 	nodeData.text = "";
		// 	foreach (string data in selectedNode.data) {
		// 		nodeData.text += "\n";
		// 		nodeData.text += data;
		// 	}
		// }
	}
}
