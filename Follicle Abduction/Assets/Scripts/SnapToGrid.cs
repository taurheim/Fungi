using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour {

	public float grid = 0.5f;
	float x = 0f, y = 0f, z = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (grid > 0f) {
			float reciprocalGrid = 1f / grid;
			x = Mathf.Round (transform.position.x * reciprocalGrid) / reciprocalGrid;
			z = Mathf.Round (transform.position.z * reciprocalGrid) / reciprocalGrid;

			transform.position = new Vector3 (x,transform.position.y,z );
		}
	}
}
