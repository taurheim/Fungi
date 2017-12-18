using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Add to component to handle button push animations
 */
public class ButtonAnimation : MonoBehaviour {

	Animation animations;

	void Start () 
	{
		animations = GetComponent<Animation>();
	}

	public void push()
	{
		animations.Play("pushed");

	}
}