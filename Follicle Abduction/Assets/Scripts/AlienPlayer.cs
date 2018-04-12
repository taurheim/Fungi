using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Alien player controller. In the process of changing the interaction from mouse-based to key-based.
 */

public class AlienPlayer : MonoBehaviour
{

	public GameObject alienIconPrefab;
	private bool captured;

	// Should be undefined at the start
	// TODO this should be private and use getters/setters for tests
	public Node currentNode;
	private GameObject alienIcon;
	private bool isMoving;
	private List<Vector3> movePositions;

	void Start ()
	{
		captured = false;
		isMoving = false;
		movePositions = new List<Vector3>();
		Debug.Log("alien player START");
		// Find our current node
		foreach(Node node in Object.FindObjectsOfTypeAll(typeof(Node))) {
			Debug.Log("found node");
			if(node.isHeadNode) {
				currentNode = node;
				alienIcon = Instantiate(alienIconPrefab, node.transform);
				break;
			}
		}
	}

	void Update() {
		if(!isMoving) {
			if(Input.GetKeyDown(KeyCode.S)) {
				moveDown();
			} else if (Input.GetKeyDown(KeyCode.W)) {
				moveUp();
			} else if (Input.GetKeyDown(KeyCode.A)) {
				moveLeft();
			} else if (Input.GetKeyDown(KeyCode.D)) {
				moveRight();
			}
		}

		if(isMoving && movePositions.Count > 0) {
			// We're moving
			if(alienIcon.transform.position == movePositions[0]) {
				// We've arrived
				movePositions.RemoveAt(0);
				if (movePositions.Count == 0) {
					Debug.Log("Done moving!");
					isMoving = false;
					currentNode.Select();
				}
			} else {
				// We still need to move more
				alienIcon.transform.position = Vector3.MoveTowards(alienIcon.transform.position, movePositions[0], 0.5f);
			}
		}
	}

	public void moveLeft() {
		moveInDirection(LineDirection.LEFT);
	}

	public void moveRight() {
		moveInDirection(LineDirection.RIGHT);
	}

	public void moveUp() {
		moveInDirection(LineDirection.UP);
	}

	public void moveDown() {
		moveInDirection(LineDirection.DOWN);
	}

	private void moveInDirection(LineDirection direction) {
		if (!currentNode) return;
		Node nextNode = currentNode.getNode(direction);
		if(nextNode) {
			currentNode.Deselect();
			movePositions.Add(currentNode.getMidPoint(direction));
			movePositions.Add(nextNode.transform.position);
			isMoving = true;
			currentNode = nextNode;
		}

	}
}
