using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Handle animations for the guards based on state.
 */
public class guardAnimation : MonoBehaviour
{

	Animation animations;

	void Start ()
	{
		animations = GetComponent<Animation> ();
	}

	public void run ()       //run animation will play on a loop, until another animation is called
	{
		animations.Play ("run_cycle", PlayMode.StopAll);
	}

	public void walk ()      //walk animation will play on a loop, until another animation is called
	{
		animations.Play ("walk_cycle", PlayMode.StopAll);
	}

	public void lookAround ()
	{
		animations.Play ("look_around", PlayMode.StopAll);
	}

	public void stop ()
	{
		animations.Play ("stop_running", PlayMode.StopAll);
	}
}
