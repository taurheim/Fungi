using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapVisibility {

	public static void setVisibilityForTag(string tag, bool visibility) {
		if (tag != "") {
			
			GameObject[] objs = GameObject.FindGameObjectsWithTag (tag);

			foreach (GameObject obj in objs) {

				MapObject mapObj = obj.GetComponent<MapObject> ();

				if (mapObj != null) {
						
					mapObj.setVisibility (visibility);

				}
			}
		}
	}
		
}
