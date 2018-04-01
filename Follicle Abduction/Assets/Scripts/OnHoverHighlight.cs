using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
	Component that enables visual highlights on mouse-hover
	Used to indicate interactivity
 */

public class OnHoverHighlight : MonoBehaviour {
	
	Renderer rend;
	Shader originalShader;
	Shader highlightShader;
	Collider objCollider;
	bool isHighlighted;
	public Color highlightColour = Color.green;

	// Use this for initialization
	void Start () {
		originalShader = gameObject.GetComponent<Renderer>().material.shader;
		highlightShader = Shader.Find("Outlined/Diffuse");
		objCollider = gameObject.GetComponent<Collider>();
		isHighlighted = false;
		rend = gameObject.GetComponent<Renderer>();
		rend.material.SetVector("_OutlineColor", highlightColour);
	}
	
	// Update is called once per frame
	void Update() {
		GameObject player = GameObject.FindGameObjectWithTag ("playerA");
		if (player) {
			RaycastHit hit;
			Camera playerCamera = player.GetComponentInChildren<Camera>();
			if (Physics.Raycast (playerCamera.transform.position, playerCamera.transform.forward, out hit, 5.0f, 1)) {
				if ((hit.collider == objCollider)) {
					if (!isHighlighted) {
						SetShader(highlightShader);
						isHighlighted = true;
					}
				} else if (isHighlighted) {
					SetShader(originalShader);
					isHighlighted = false; 
				}
			} else if (isHighlighted) {
				SetShader(originalShader);
				isHighlighted = false; 
			}
		}	
	}

	void SetShader(Shader newShader) {
		rend = gameObject.GetComponent<Renderer>();
		if (rend) {
			rend.material.shader = newShader;
			Debug.Log("set shader!");
			Debug.Log(rend.material.shader);
		}
	}
}
