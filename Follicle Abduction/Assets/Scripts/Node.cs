using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum NodeState
{
	LOCKED, // Cannot be accessed
	UNLOCKED, // Available to be hacked
	COMPLETED // Node is completed and can be "used"
};

/*
	A map node for the alien. Can be in 3 states that are modified by the alien clicking on the icon. Nodes can have children which
	they will be connected to programmatically. For more information see design doc.
 */
public class Node : NetworkBehaviour
{

	private enum LineDirection { UP, DOWN, LEFT, RIGHT, NONE};

	public NodeState state = NodeState.LOCKED;
	public bool isSelected;
	public bool isHeadNode; // Should only be set on one node per level

	// Upon completion, all child nodes will unlock
	public Node[] childNodes;

	// Upon completion, show all objects with this tag.
	public string tagForHiddenObjects;

	// Data held by the node made visible by selection
	public string[] data;

	// This is a reference to the progress bar of the node's completion
	public GameObject progressBar;
	public GameObject outline;

	// True if the node is actively being hacked
	private bool isHacking;

	private int percentComplete = 0;

	// Lines from this node to all of its children
	private LineRenderer[] nodeLines;
	
	private float fadePerSecond = 0.5F;

	private KeyCode actionKey = KeyCode.Space;

	private Dictionary<LineDirection, Node> connectedNodes;

	/* 
		Override these for node-specific functionality
	 */
	public virtual void onStartAction ()
	{
		// Called when a node is unlocked and the action button is pressed
	}

	public virtual void onEndAction ()
	{
		// Called when a node is unlocked and the action button is released
	}

	public virtual void onUnlock () {
		// Called when a node becomes unlocked
	}

	public virtual void onComplete() {
		// Called when a node is completed
	}

	void Start ()
	{
		// Draw a line from this node to all children
		if(isHeadNode) {
			Select();
			drawLinesToChildren(LineDirection.NONE, null);
		}
	}

	public virtual void Select ()
	{
		isSelected = true;
	}

	public virtual void Deselect ()
	{
		isSelected = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Show the border if we're selected
		if(outline) {
			if(isSelected) {
				outline.SetActive(true);
			} else {
				outline.SetActive(false);
			}
		}

		if (isHacking && state == NodeState.UNLOCKED) {
			if (percentComplete < 100f) {
				percentComplete++;
				float amt = percentComplete / 100f;

				progressBar.transform.localPosition = new Vector3 (amt / 2 - 0.5f, progressBar.transform.localPosition.y, progressBar.transform.localPosition.z);
				progressBar.transform.localScale = new Vector3 (amt, 1, 1);
			} else {
				completeNode();
			}
		}

		if (isSelected && Input.GetKeyDown(actionKey)) {
			if (state == NodeState.UNLOCKED) {
				// This will get the above to run
				isHacking = true; 
			} else if (state == NodeState.COMPLETED) {
				// If we're completed, then we can run the action
				onStartAction();
			}
		} else if (isSelected && Input.GetKeyUp(actionKey)) {
			if (state == NodeState.UNLOCKED) {
				isHacking = false;
			} else if (state == NodeState.COMPLETED) {
				// Stop the action
				onEndAction();
			}
		}
	}

	public void unlockNode ()
	{
		state = NodeState.UNLOCKED;
	}

	public Vector2 getPosition ()
	{
		return new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
	}

	public void completeNode ()
	{
		// Can't complete without unlocking
		if (state == NodeState.UNLOCKED) {
			state = NodeState.COMPLETED;

			// Unlock all child nodes
			for (int i = 0; i < childNodes.Length; i++) {
				childNodes [i].unlockNode ();
			}

			// Mark all connections as green
			Color currentColor = progressBar.GetComponent<MeshRenderer> ().material.color;
			Color newColor = new Color (0F, 1.0F, 0F);

			foreach (LineRenderer line in nodeLines) {
				line.startColor = newColor;
				line.endColor = newColor;
			}
			progressBar.GetComponent<MeshRenderer> ().material.color = newColor;
			GetComponent<MeshRenderer> ().enabled = false;

			// Run any extra completion code
			onComplete();
		}
	}

	void recursivelyDrawLinesToChildren() {
		// This should only be called by the first node - it will call the drawLinesToChildren() method on all the other nodes

	}

	void drawLinesToChildren (LineDirection parentDirection, Node parent)
	{
		// Most nodes will only have 3 child nodes (6 lines)
		// The first node can have 4 (8 lines)
		connectedNodes = new Dictionary<LineDirection, Node>();
		nodeLines = new LineRenderer[8];

		if(parentDirection != LineDirection.NONE) {
			connectedNodes[parentDirection] = parent;
		}

		for(int i = 0; i < childNodes.Length; i++) {
			// Figure out which directions each child could go
			var possibleDirections = new List<LineDirection>();

			Node childNode = childNodes[i];
			Vector2 parentPosition = this.getPosition();
			Vector2 childPosition = childNode.getPosition();

			if (parentPosition.x == childPosition.x) {
				// Can only have a vertical line
				LineDirection outDirection = (childPosition.y > parentPosition.y) ? LineDirection.UP : LineDirection.DOWN;
				possibleDirections.Add(outDirection);
			} else if(parentPosition.y == childPosition.y) {
				// Can only have a horizontal line
				LineDirection outDirection = (childPosition.x > parentPosition.x) ? LineDirection.RIGHT : LineDirection.LEFT;
				possibleDirections.Add(outDirection);
			} else {
				// Two lines possible
				LineDirection horizontal = (childPosition.x > parentPosition.x) ? LineDirection.RIGHT : LineDirection.LEFT;
				LineDirection vertical = (childPosition.y > parentPosition.y) ? LineDirection.UP : LineDirection.DOWN;
				possibleDirections.Add(vertical);
				possibleDirections.Add(horizontal);
			}

			// We now know which directions are possible, so let's try to fill them in
			bool wasPlaced = false;
			for(int d = 0; d < possibleDirections.Count; d++) {
				if(!wasPlaced && !connectedNodes.ContainsKey(possibleDirections[d])) {
					// We have a spot for it
					connectedNodes[possibleDirections[d]] = childNode;
					wasPlaced = true;
				}
			}

			if(!wasPlaced) {
				Debug.Log("Couldn't place node!");
				Debug.Log("Possible directions: " + possibleDirections.Count);
				if(possibleDirections.Count == 2) {
					Debug.Log(possibleDirections[0].ToString());
					Debug.Log(possibleDirections[1].ToString());
				}
				Debug.Log("UP: " + connectedNodes.ContainsKey(LineDirection.UP));
				Debug.Log("DOWN: " + connectedNodes.ContainsKey(LineDirection.DOWN));
				Debug.Log("LEFT: " + connectedNodes.ContainsKey(LineDirection.LEFT));
				Debug.Log("RIGHT: " + connectedNodes.ContainsKey(LineDirection.RIGHT));
				Debug.Log(parentPosition);
				Debug.Log(childPosition);
				Debug.LogError("Couldn't place a node. This is likely because there is another child node close to it. Each node can only have one child node in each direction"); 
				return;
			}
		}

		// Go through the connected nodes and draw lines to them
		// Also recursively call this method
		foreach(LineDirection dir in System.Enum.GetValues(typeof(LineDirection))) {
			if (dir == parentDirection) continue; // Line has already been drawn
			if (!connectedNodes.ContainsKey(dir)) continue; // No line needed

			Node childNode = connectedNodes[dir];
			Vector2 parentPosition = getPosition();
			Vector2 childPosition = childNode.getPosition();
			int i = ((int) dir)*2;

			// Draw Lines
			if (parentPosition.x == childPosition.x || parentPosition.y == childPosition.y) {
				// Only need one line
				GameObject lineObject = new GameObject ();
				lineObject.layer = LayerMask.NameToLayer ("Minimap");
				nodeLines [i] = makeLine (lineObject, parentPosition, childPosition);
			} else {
				// Vertical line
				GameObject verticalLineObject = new GameObject ();
				GameObject horizontalLineObject = new GameObject ();
				verticalLineObject.layer = LayerMask.NameToLayer ("Minimap");
				horizontalLineObject.layer = LayerMask.NameToLayer ("Minimap");
				Vector3 midPoint ;
				if(dir == LineDirection.UP || dir == LineDirection.DOWN) {
					// Vertical then horizontal
					midPoint = new Vector2 (parentPosition.x, childPosition.y);
				} else {
					// Horizontal then vertical
					midPoint = new Vector2 (childPosition.x, parentPosition.y);
				}
				nodeLines [i] = makeLine (verticalLineObject, parentPosition, midPoint);
				nodeLines [i+1] = makeLine (horizontalLineObject, midPoint, childPosition);
			}

			// Recurse
			LineDirection entryDirection = getEntryDirection(dir, parentPosition, childPosition);
			childNode.drawLinesToChildren(entryDirection, this);
		}
	}

	/*
		Assuming you have two nodes, with the exit direction known, find where the line should enter on the child
	 */
	static LineDirection getEntryDirection(LineDirection exitDirection, Vector2 exitPosition, Vector2 entryPosition) {
		if (entryPosition.x == exitPosition.x) {
			// If we're not moving horizontally, then we have either a vertical line up (entering the bottom) or a vertical line down (entering the top)
			return (entryPosition.y > exitPosition.y) ? LineDirection.DOWN : LineDirection.UP;
		} else if (entryPosition.y == exitPosition.y) {
			// If we're not moving vertically, we have either horizontal line right (entering left) or left (entering right)
			return (entryPosition.x > exitPosition.x) ? LineDirection.LEFT : LineDirection.RIGHT;
		} else {
			// Otherwise, our entry point is on the opposite axis
			if(exitDirection == LineDirection.RIGHT || exitDirection == LineDirection.LEFT) {
				return (entryPosition.y > exitPosition.y) ? LineDirection.DOWN : LineDirection.UP;
			} else {
				return (entryPosition.x > exitPosition.x) ? LineDirection.LEFT : LineDirection.RIGHT;
			}
		}
	}

	LineRenderer makeLine (GameObject obj, Vector2 startPosition, Vector2 endPosition)
	{
		// TODO move all this to constants at the top
		LineRenderer line = obj.AddComponent<LineRenderer> ();
		line.startWidth = 0.5F;
		line.endWidth = 0.5F;
		line.positionCount = 2;
		line.SetPosition (0, new Vector3(startPosition.x, gameObject.transform.position.y, startPosition.y));
		line.SetPosition (1, new Vector3(endPosition.x, gameObject.transform.position.y, endPosition.y));
		line.material = new Material (Shader.Find ("Particles/Additive"));
		line.startColor = Color.red;
		line.endColor = Color.red;

		return line;
	}

}