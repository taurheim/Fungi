using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	public int cellSize;
	public int n;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos () {
		DrawGrid ();
	}

	private void DrawGrid(){
		int size = cellSize * n;
		int currentPoint;
		for (int i = 0; i <= n; i ++){
			currentPoint = cellSize * i;
			Vector3 startX = new Vector3 (currentPoint, 0, 0);
			Vector3 endX = new Vector3 (currentPoint, 0, size);
			Gizmos.DrawLine (startX, endX);

			Vector3 startZ = new Vector3 (0, 0, currentPoint);
			Vector3 endZ = new Vector3 (size, 0, currentPoint);
			Gizmos.DrawLine (startZ, endZ);
		}
	}
}
