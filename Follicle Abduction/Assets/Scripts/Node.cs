using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState { 
	LOCKED, 	// Cannot be accessed
	UNLOCKED, 	// Available to be hacked
	COMPLETED	// Completed
};

public class Node : MonoBehaviour  {

	public NodeState state = NodeState.LOCKED;

	// Used for the demo - this is the key to press to hack this node
	public string hackKey;

	// Upon completion, all child nodes will unlock
	public Node[] childNodes;

	// Upon completion, show all objects with this tag.
	public string tagForHiddenObjects;

	// This is a reference to the progress bar of the node's completion
	//TODO dynamically generate this
	public GameObject progressBar;

	// True if the node is actively being hacked
	private bool isHacking;

	int percentComplete = 0;

	// Lines from this node to all of its children
	private LineRenderer[] nodeLines;
	
	private float fadePerSecond = 0.5F;

	void Start () {
		//Hide map icons with given tag
		MapVisibility.setVisibilityForTag (tagForHiddenObjects, false);

		// Draw a line from this node to all children
		drawLinesToChildren();
	}

	void OnMouseDown() {
		HandleMouseDown();
	}

	void OnMouseUp() {
		HandleMouseUp();
	}

	public virtual void HandleMouseDown() {
		isHacking = true;
	}

	public virtual void HandleMouseUp() {
		isHacking = false;
	}
	
	// Update is called once per frame
	void Update () {
		// If the node is locked, do nothing
		if(state == NodeState.UNLOCKED) {

			if(isHacking && percentComplete < 100f) {
				percentComplete++;
				float amt = percentComplete / 100f;

				progressBar.transform.localPosition = new Vector3(amt/2 - 0.5f, progressBar.transform.localPosition.y, progressBar.transform.localPosition.z);
				progressBar.transform.localScale = new Vector3(amt, 1, 1);
			}

			if(percentComplete >= 100f) {
				completeNode();
			}
		} else if(state == NodeState.COMPLETED) {
			// Mark all connections as green, fade them out
			Color currentColor = progressBar.GetComponent<MeshRenderer>().material.color;
			Color newColor = new Color(0F, 1.0F, 0F, currentColor.a - (fadePerSecond * Time.deltaTime));

			foreach(LineRenderer line in nodeLines){
				line.startColor = newColor;
				line.endColor = newColor;
			}
			progressBar.GetComponent<MeshRenderer>().material.color = newColor;
			GetComponent<MeshRenderer>().enabled = false;

			// Finally, hide the node from view
			if(newColor.a <= 0) {
				gameObject.SetActive(false);
			}
		}
	}

	public void unlockNode() {
		state = NodeState.UNLOCKED;
	}

	public Vector3 getPosition() {
		return gameObject.transform.position;
	}

	public void completeNode() {
		// Can't complete without unlocking
		if(state == NodeState.UNLOCKED) {
			state = NodeState.COMPLETED;

			// Show all hidden objects
			if(tagForHiddenObjects != null) {
				Debug.Log(tagForHiddenObjects.Length);
				MapVisibility.setVisibilityForTag (tagForHiddenObjects, true);
			}

			// Unlock all child nodes
			for(int i=0;i<childNodes.Length;i++){
				childNodes[i].unlockNode();
			}
		}
	}

	void drawLinesToChildren() {
		// The maximum number of lines is 2*child nodes
		nodeLines = new LineRenderer[childNodes.Length*2];
		
		for(int i=0;i<childNodes.Length;i++){
			Node childNode = childNodes[i];
			Vector3 parentPosition = this.getPosition();
			Vector3 childPosition = childNode.getPosition();

			// TODO magic #s, strings, duplication
			// TODO maybe make this in a range instead of exact?
			// TODO is there a better way than creating a new game object every time? Can we store them somewhere?
			if(parentPosition.x == childPosition.x || parentPosition.y == childPosition.y) {
				// Only need one line
				GameObject lineObject = new GameObject();
				lineObject.layer = LayerMask.NameToLayer("Minimap");
				nodeLines[i] = makeLine(lineObject, parentPosition, childPosition);
			} else {
				// Vertical line
				// TODO draw different lines first and second based on positions?
				// TODO make sure lines don't cross?
				GameObject verticalLineObject = new GameObject();
				GameObject horizontalLineObject = new GameObject();
				verticalLineObject.layer = LayerMask.NameToLayer("Minimap");
				horizontalLineObject.layer = LayerMask.NameToLayer("Minimap");
				// Vertical then horizontal
				Vector3 midPoint = new Vector3(parentPosition.x, childPosition.y, childPosition.z);
				nodeLines[i] = makeLine(verticalLineObject, parentPosition, midPoint);
				nodeLines[i+1]  = makeLine(horizontalLineObject, midPoint, childPosition);
			}
		}
	}

	LineRenderer makeLine(GameObject obj, Vector3 startPosition, Vector3 endPosition) {
		// TODO move all this to constants at the top
		LineRenderer line = obj.AddComponent<LineRenderer>();
		line.startWidth = 0.2F;
		line.endWidth = 0.2F;
		line.positionCount = 2;
		line.SetPosition(0, startPosition);
		line.SetPosition(1, endPosition);
		line.material = new Material (Shader.Find("Particles/Additive"));
		line.startColor = Color.red;
		line.endColor = Color.red;

		return line;
	}

}