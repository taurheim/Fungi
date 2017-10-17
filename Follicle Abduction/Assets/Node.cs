using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public string hackKey;

	public bool isUnlocked = true;
	public bool isCompleted = false;

	public GameObject[] childNodes;

	public GameObject[] toShow;
	public GameObject[] toHide;

	public GameObject progressBar;

	bool isHacking;

	int percentComplete = 0;

	void Start () {
		
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
		// Show everything that should be shown
		for(int i=0;i<toShow.Length;++i) {
			toShow[i].SetActive(true);
		}

		// Hide everything that should be hidden
		for(int i=0;i<toHide.Length;++i) {
			toHide[i].SetActive(false);
		}

		// Unlock all child nodes
		for(int i=0;i<childNodes.Length; ++i) {
			childNodes[i].GetComponent<Node>().isUnlocked = true;
		}

		// Finally, hide the node from view
		gameObject.SetActive(false);
	}
}
