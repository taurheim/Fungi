﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Custom puzzle implementation for the door matching puzzle.
 */
public class Symbol_Match_Door_Puzzle : MonoBehaviour
{

	public GameObject CorrectButton;
	public GameObject Door;

	private ButtonPress CorrectButtonScript;
	private DoorLogic DoorScript;

	// Use this for initialization
	void Start ()
	{
		CorrectButtonScript = CorrectButton.GetComponent<ButtonPress> ();
		DoorScript = Door.GetComponent<DoorLogic> ();
	}
	
	// Check if the correct button is pressed, if so, open the door
	void Update ()
	{
		//Debug.Log(CorrectButtonScript.getButtonStatus());
		//(CorrectButtonScript.getButtonStatus()) { DoorScript.startOpening(); }
	}
}
