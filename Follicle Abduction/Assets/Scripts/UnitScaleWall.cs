using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UnitScaleWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector4 size = GetComponent<Renderer>().bounds.size;
		Vector3 scale = transform.localScale;
		float scaleFactor = 1.0f / size.x;
		scale.x = scaleFactor;
		scale.y = scaleFactor;
		scale.z = scaleFactor;
		transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
