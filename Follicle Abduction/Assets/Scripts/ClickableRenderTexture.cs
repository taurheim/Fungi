using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Forwards clicks through a render texture.

	A render texture presents a camera's view on as a texture. By intercepting the clicks on the texture and
	raycasting them forward, we can simulate clicking. Specifically, clicking will call the

	Node.HandleMouseDown() method on whatever node is clicked through the render texture.
 */
public class ClickableRenderTexture : MonoBehaviour
{

	// TODO cleanup the duplication here
	public Camera securityCamera;
	public Camera mainCamera;

	void OnMouseDown ()
	{
		if (!mainCamera) {
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}

		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {

			var localPoint = hit.textureCoord;

			Ray portalRay = securityCamera.ScreenPointToRay (new Vector2 (localPoint.x * securityCamera.pixelWidth, localPoint.y * securityCamera.pixelHeight));
			RaycastHit portalHit;
			// test these camera coordinates in another raycast test
			if (Physics.Raycast (portalRay, out portalHit)) {
				GameObject collidedWith = portalHit.collider.gameObject;

				if (collidedWith.GetComponent<Node> ()) {
					collidedWith.GetComponent<Node> ().HandleMouseDown ();
				}
			}
		}
	}

	void OnMouseUp ()
	{
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {

			var localPoint = hit.textureCoord;

			Ray portalRay = securityCamera.ScreenPointToRay (new Vector2 (localPoint.x * securityCamera.pixelWidth, localPoint.y * securityCamera.pixelHeight));
			RaycastHit portalHit;
			// test these camera coordinates in another raycast test
			if (Physics.Raycast (portalRay, out portalHit)) {
				GameObject collidedWith = portalHit.collider.gameObject;

				if (collidedWith.GetComponent<Node> ()) {
					collidedWith.GetComponent<Node> ().HandleMouseUp ();
				}
			}
		}
	}
}
