using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Handles visiblity/toggle for objects that are shown on the map(alien view).
	Needed so that the alien only sees unlocked nodes.
 */
public class MapObject : MonoBehaviour
{

	public GameObject mapIcon;
	private Renderer mapIconRenderer;

	void Awake ()
	{
		if (mapIcon != null) {
			mapIconRenderer = mapIcon.GetComponent<Renderer> ();
		}
	}

	public void setMapIcon (GameObject newMapIcon)
	{
		mapIcon = newMapIcon;
		mapIconRenderer = mapIcon.GetComponent<Renderer> ();
	}

	public void setVisibility (bool visibility)
	{
		if (mapIconRenderer == null)
			return;
		mapIconRenderer.enabled = visibility;
	}

	public bool isVisible ()
	{
		if (mapIconRenderer == null)
			return false;
		return mapIconRenderer.enabled;
	}

}
