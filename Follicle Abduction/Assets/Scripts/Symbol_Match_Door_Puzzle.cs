using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Match_Door_Puzzle : MonoBehaviour {

    public GameObject CorrectButton;
    public GameObject Door;

    private ButtonPress CorrectButtonScript;
    private DoorLogic DoorScript;

	// Use this for initialization
	void Start () {
        CorrectButtonScript = CorrectButton.GetComponent<ButtonPress>();
        DoorScript = Door.GetComponent<DoorLogic>();
    }
	
	// Check if the correct button is pressed, if so, open the door
	void Update () {
        //Debug.Log(CorrectButtonScript.getButtonStatus());
        if (CorrectButtonScript.getButtonStatus()) { DoorScript.startOpening(); }
	}
}
