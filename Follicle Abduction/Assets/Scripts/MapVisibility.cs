using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapVisibility {

	public static void setVisibilityForTag(string tag, bool visibility) {
		if (tag != "") {
			
			GameObject[] objs = GameObject.FindGameObjectsWithTag (tag);

			foreach (GameObject obj in objs) {

				MonoBehaviour.print (obj.name);
				
				foreach (Transform child in obj.transform) {
					
					if (child.CompareTag ("mapIcon")) {
						
						child.GetComponent<Renderer> ().enabled = visibility;

					}
				}
			}
		}
	}
}
