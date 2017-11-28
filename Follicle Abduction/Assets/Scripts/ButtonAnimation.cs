using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour {

	Animation animations;

	//bool pushed = false;

	void Start () 
	{
		animations = GetComponent<Animation>();
	}

	public void push()
	{

		animations.Play("pushed");

	}
}