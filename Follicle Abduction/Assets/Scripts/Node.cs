using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public string hackKey;

	public bool isUnlocked = true;
	public bool isCompleted = false;

	public GameObject[] childNodes;

	public string tagForHiddenObjects;

	public GameObject progressBar;

	bool isHacking;

	int percentComplete = 0;

	private LineRenderer[] nodeLines;
	
	private float fadePerSecond = 0.5F;

	void Start () {
		//Hide map icons with given tag
		MapVisibility.setVisibilityForTag (tagForHiddenObjects, false);

		// Draw a line from this node to all children
		nodeLines = new LineRenderer[childNodes.Length];
		
		for(int i=0;i<nodeLines.Length;i++){
			LineRenderer line = this.gameObject.AddComponent<LineRenderer>();
			line.startWidth = 0.2F;
			line.endWidth = 0.2F;
			line.positionCount = 2;
			line.SetPosition(0, gameObject.transform.position);
			line.SetPosition(1, childNodes[i].transform.position);
      		line.material = new Material (Shader.Find("Particles/Additive"));
			line.startColor = Color.red;
			line.endColor = Color.red;

			nodeLines[i] = line;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if(isUnlocked) {
			checkForHackKey();

			if(isHacking && percentComplete < 100f) {
				percentComplete++;
				float amt = percentComplete / 100f;

				progressBar.transform.localPosition = new Vector3(amt/2 - 0.5f, progressBar.transform.localPosition.y, progressBar.transform.localPosition.z);
				progressBar.transform.localScale = new Vector3(amt, 1, 1);
			}

			if(!isCompleted && percentComplete >= 100f) {
				completeNode();
			}
		} else {
		}
	}

	//TODO determine if this is actually the best way of doing this. Maybe polling for the key with a timer on increments is better?
	// If we press the key, start hacking
	void checkForHackKey() {
		if (Input.GetKeyDown(hackKey)){
			isHacking = true;
		}

		// When we let go of the key, stop hacking
		if(Input.GetKeyUp(hackKey)) {
			isHacking = false;
		}
	}

	void completeNode() {
		MapVisibility.setVisibilityForTag (tagForHiddenObjects, true);

		// Unlock all child nodes
		for(int i=0;i<childNodes.Length; ++i) {
			childNodes[i].GetComponent<Node>().isUnlocked = true;
		}


		// Mark all connections as green, fade them out
		Color currentColor = progressBar.GetComponent<MeshRenderer>().material.color;
		Color newColor = new Color(0F, 255F, 0F, currentColor.a - (fadePerSecond * Time.deltaTime));
		Debug.Log(newColor.a);

		foreach(LineRenderer line in nodeLines){
			line.startColor = newColor;
			line.endColor = newColor;
		}
		progressBar.GetComponent<MeshRenderer>().material.color = newColor;

		// Finally, hide the node from view
		if(newColor.a <= 0) {
			gameObject.SetActive(false);
		}
	}

}