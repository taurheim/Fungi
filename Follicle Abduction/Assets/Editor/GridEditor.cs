using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(Grid))]
public class GridEditor : Editor
{

	Grid grid;
	GameObject currentObj;
	Object currentPrefab;
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
		GUILayout.Label(" INSTRUCTIONS: \n Drag an initial prefab into the scene. \n With the prefab selected, hit A to duplicate it. \n " +
			" Use the mouse to place it in a grid cell. \n Hit S to place a duplicate in the scene. \n Hit D to cancel for the current prefab. \n" +
			" Hit W to rotate 90 degrees");
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(" Cell Size ");
		grid.cellSize = EditorGUILayout.IntField(grid.cellSize, GUILayout.Width(50));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label(" Number of Cells ");
		grid.n = EditorGUILayout.IntField(grid.n, GUILayout.Width(50));
		GUILayout.EndHorizontal();
	}

	void GridUpdate(SceneView sceneview)
	{
		Event e = Event.current;
		if (currentObj) {
			if (e.isKey && (e.character == 'd')) { //cancel
				MonoBehaviour.print ("D");
				DestroyImmediate (currentObj);
				currentObj = null;
			} else if (e.isMouse) { //find grid position based on mouse
				Vector3 intersection = MousePlaneIntersection (e);
				Vector3 gridPosition = GridPositionFromPoint (intersection);
				currentObj.transform.position = gridPosition;
			} else if (e.isKey && (e.character == 's')) { //place object in scene
				MonoBehaviour.print ("S");
				PlaceDuplicateFromCurrentSelection ();
			} else if (e.isKey && (e.character == 'w')) { //rotate object 90 degrees
				currentObj.transform.Rotate (
					currentObj.transform.rotation.x,
					currentObj.transform.rotation.y + 90,
					currentObj.transform.rotation.z);
			}
		} else if (e.isKey && (e.character == 'a')) { //select prefab
			MonoBehaviour.print ("A");
			currentPrefab = EditorUtility.GetPrefabParent(Selection.activeObject);
			if (currentPrefab) {
				currentObj = (GameObject)PrefabUtility.InstantiatePrefab (currentPrefab);
			} else {
				MonoBehaviour.print ("no prefab");
			}
		}
	}

	private Vector3 MousePlaneIntersection (Event currentEvent) {
		//Create ray from mouse
		Vector3 mousePos = new Vector3 (currentEvent.mousePosition.x, currentEvent.mousePosition.y);
		Ray ray = HandleUtility.GUIPointToWorldRay (mousePos);
		//Find the distance along the ray to the XZ plane
		Plane xz = new Plane (Vector3.up, Vector3.zero);
		float distance;
		xz.Raycast (ray, out distance);
		//Get the point of intersection with the XZ plane
		Vector3 intersection = ray.GetPoint (distance);
		return intersection;
	}

	private Vector3 GridPositionFromPoint (Vector3 point) {
		//Find grid cell
		int gridX = Mathf.FloorToInt (point.x / grid.cellSize);
		int gridZ = Mathf.FloorToInt (point.z / grid.cellSize);
		Vector3 position = new Vector3 (
			grid.cellSize * gridX + grid.cellSize / 2.0f,
			currentObj.GetComponent<Renderer> ().bounds.size.y / 2.0f,
			grid.cellSize * gridZ + grid.cellSize / 2.0f);
		return position;
	}
	
	private void PlaceDuplicateFromCurrentSelection () {
		objs.Add ((GameObject)PrefabUtility.InstantiatePrefab (currentPrefab));
		GameObject newObj = (GameObject)objs [objs.Count - 1];
		newObj.transform.position = new Vector3 (
			currentObj.transform.position.x,
			currentObj.transform.position.y,
			currentObj.transform.position.z);
		newObj.transform.rotation = currentObj.transform.rotation;
	}

}