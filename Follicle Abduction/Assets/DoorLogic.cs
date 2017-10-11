using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour {

    private bool isLocked;
    private bool isOpen;
    private float moveSpeed; // How fast door opens/closes
    public float closeDistance;

    public GameObject RightDoor;
    public GameObject LeftDoor;
    public GameObject Player;

	void Start () {
        isLocked = false;
        RightDoor = GameObject.Find("Right_Door");
        LeftDoor = GameObject.Find("Left_Door");
        Player = GameObject.Find("FirstPersonCharacter");
        this.isOpen = true;
        this.moveSpeed = 0.5f;
        // Assuming intially closed
        this.closeDistance = Vector3.Distance(RightDoor.transform.position, LeftDoor.transform.position);

    }
	
	void Update () {
        // Close or open door based on isOpen status
        if (this.isOpen) {
            this.open();
        }
        else {
            this.close();
        }

        // Auto start opening if player is nearby
        if (this.checkPlayerInVicinity()) {
            this.isOpen = true;
        }
        else {
            this.isOpen = false;
        }
    }

    // Called every frame if door is opening
    void open() {
        if (Vector3.Distance(RightDoor.transform.position, LeftDoor.transform.position) < 6) {
            RightDoor.transform.Translate(-(Vector3.forward * this.moveSpeed) * Time.deltaTime);
            LeftDoor.transform.Translate((Vector3.forward * this.moveSpeed) * Time.deltaTime);
        }
    }

    // Called every frame if door is closing
    void close() {
        if (Vector3.Distance(RightDoor.transform.position, LeftDoor.transform.position) > this.closeDistance) {
            RightDoor.transform.Translate((Vector3.forward * this.moveSpeed) * Time.deltaTime);
            LeftDoor.transform.Translate(-(Vector3.forward * this.moveSpeed) * Time.deltaTime);
        }
    }


    /* TODO
    - Check player vicinity less frequently (minimize lag)
    - Compare player distance to distance of middle of doors (rather than right)
    */
    bool checkPlayerInVicinity() {
        System.Console.Out.WriteLine(this.transform.position);
        System.Console.Out.WriteLine(Player.transform.position + "\n");

        if (Vector3.Distance(RightDoor.transform.position, Player.transform.position) < 5) {
            return true;
        }
        else {
            return false;
        }
    }
}
