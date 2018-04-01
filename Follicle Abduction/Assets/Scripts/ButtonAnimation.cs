using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	TODO: Just stick this in ButtonPress.cs
	Add to component to handle button push animations
 */
public class ButtonAnimation : MonoBehaviour
{

	Animation animations;

	void Start ()
	{
		animations = GetComponent<Animation> ();
	}

	// Push button
	public void push ()
	{
		animations.Play ("pushed");

	}
}