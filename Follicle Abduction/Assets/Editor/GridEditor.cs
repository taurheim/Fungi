using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(Grid))]
public class GridEditor : Editor
{

	Grid grid;
	GameObject currentObj;
	ArrayList objs;

	public void OnEnable()
	{
		grid = (Grid)target;
		SceneView.onSceneGUIDelegate = GridUpdate;
		objs = new ArrayList ();
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(" Cell Size ");
		grid.cellSize = EditorGUILayout.IntField(grid.cellSize, GUILayout.Width(50));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(" Number of Cells ");
		grid.n = EditorGUILayout.IntField(grid.n, GUILayout.Width(50));
		GUILayout.EndHorizontal();
	}

//	void OnSceneGUI() {
//		
//	}

	void GridUpdate(SceneView sceneview)
	{
		Event e = Event.current;

		if (currentObj) {

			if (e.isKey && (e.character == 'd')) {
				MonoBehaviour.print ("D");
				DestroyImmediate (currentObj);
				currentObj = null;
			} else if (e.isMouse) {
				//Create ray from mouse
				Vector3 mousePos = new Vector3 (e.mousePosition.x, e.mousePosition.y);
				Ray ray = HandleUtility.GUIPointToWorldRay (mousePos);
				//Find the distance along the ray to the XZ plane
				Plane xz = new Plane (Vector3.up, Vector3.zero);
				float distance;
				xz.Raycast (ray, out distance);
				//Get the point of intersection with the XZ plane
				Vector3 intersection = ray.GetPoint (distance);
				//				MonoBehaviour.print (intersection);
				//Find grid cell
				int gridX = Mathf.FloorToInt (intersection.x / grid.cellSize);
				int gridZ = Mathf.FloorToInt (intersection.z / grid.cellSize);
				Vector3 position = new Vector3 (
					grid.cellSize * gridX + grid.cellSize / 2.0f,
					currentObj.GetComponent<Renderer> ().bounds.size.y / 2.0f,
					grid.cellSize * gridZ + grid.cellSize / 2.0f);
				currentObj.transform.position = position;
			} else if (e.isKey && (e.character == 's')) {
				MonoBehaviour.print ("S");
				objs.Add (GameObject.CreatePrimitive (PrimitiveType.Cube));
				GameObject newObj = (GameObject)objs [objs.Count - 1];
				newObj.transform.position = new Vector3 (
					currentObj.transform.position.x,
					currentObj.transform.position.y,
					currentObj.transform.position.z);
				newObj.transform.rotation = currentObj.transform.rotation;
				newObj.transform.localScale = new Vector3 (5, 5, 1);
			} else if (e.isKey && (e.character == 'w')) {
				currentObj.transform.Rotate (
					currentObj.transform.rotation.x,
					currentObj.transform.rotation.y + 90,
					currentObj.transform.rotation.z);
			}
		} else if (e.isKey && (e.character == 'a')) {
			MonoBehaviour.print ("A");
			Object prefab = EditorUtility.GetPrefabParent(Selection.activeObject);
			if (prefab) {
				MonoBehaviour.print (prefab);
			} else {
				MonoBehaviour.print ("no prefab");
			}
			currentObj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			currentObj.transform.localScale = new Vector3 (5, 5, 1);
			//currentObj.GetComponent<Renderer> ().material.color = new Color (1.0f, 0.0f, 0.0f, 0.5f);
		}
	}


}