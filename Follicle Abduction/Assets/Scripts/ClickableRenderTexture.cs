using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableRenderTexture : MonoBehaviour {

	// Yay code duplication! Clean this up.
	public Camera securityCamera; 
	public Camera mainCamera;

     void OnMouseDown () 
     {
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit)) 
		{

			var localPoint = hit.textureCoord;

			Ray portalRay = securityCamera.ScreenPointToRay(new Vector2(localPoint.x * securityCamera.pixelWidth, localPoint.y * securityCamera.pixelHeight));
			RaycastHit portalHit;
			// test these camera coordinates in another raycast test
			if(Physics.Raycast(portalRay, out portalHit))
			{
				GameObject collidedWith = portalHit.collider.gameObject;

				if(collidedWith.GetComponent<Node>()){
					collidedWith.GetComponent<Node>().HandleMouseDown();
				}
			}
		}
	}

	void OnMouseUp() {
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit)) 
		{

			var localPoint = hit.textureCoord;

			Ray portalRay = securityCamera.ScreenPointToRay(new Vector2(localPoint.x * securityCamera.pixelWidth, localPoint.y * securityCamera.pixelHeight));
			RaycastHit portalHit;
			// test these camera coordinates in another raycast test
			if(Physics.Raycast(portalRay, out portalHit))
			{
				GameObject collidedWith = portalHit.collider.gameObject;

				if(collidedWith.GetComponent<Node>()){
					collidedWith.GetComponent<Node>().HandleMouseUp();
				}
			}
		}
	}
}
