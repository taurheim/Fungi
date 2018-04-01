using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Handle suspiciousness
	TODO: Still a stub, build out once suspiciousness is fully implemented
 */
public class Alien : MonoBehaviour
{

	public GameObject alienIconPrefab;

	public int maxSuspiciousness = 100;
	private int suspiciousness;
	private bool captured;

	// Should be undefined at the start
	// TODO this should be private and use getters/setters for tests
	public Node currentNode;

	private GameObject alienIcon;

	private bool isMoving;
	private List<Vector3> movePositions;

	void Start ()
	{
		suspiciousness = 0;
		captured = false;
		isMoving = false;
		movePositions = new List<Vector3>();

		// Find our current node
		foreach(Node node in FindObjectsOfType(typeof(Node))) {
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

	public void doSomethingSuspicious (int howSuspicious)
	{
		suspiciousness += howSuspicious;
		if (suspiciousness >= maxSuspiciousness) {
			captured = true;
		}
	}

	public int getSuspiciousness ()
	{
		return suspiciousness;
	}

	public bool hasBeenCaptured ()
	{
		return captured;
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
