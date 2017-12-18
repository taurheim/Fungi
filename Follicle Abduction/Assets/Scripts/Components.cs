using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
	Used to enable and disable components on an entity
	TODO: Remove this entirely - our players shouldn't depend on this, all we need to do is enable and disable the cameras and sound listeners
 */
public class Components : MonoBehaviour {

	[SerializeField]
	Behaviour[] components;


	public Behaviour[] GetComponentList()
	{
		return components;
	}
}
