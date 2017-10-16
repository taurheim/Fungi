using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour {

    private bool isLocked; // Unused atm, maybe unnecessary
    private bool isOpen;
    private float moveSpeed; // How fast door opens/closes (higher = faster)
    public float closeDistance;

    // Select these in Unity UI
    public GameObject RightDoor;
    public GameObject LeftDoor;
    public GameObject Player;

    public GameObject Icon;

	void Start () {
        isLocked = false;
        this.isOpen = false;
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
        // This can be swapped out for other opening methods
        /*
        if (this.checkPlayerInVicinity()) {
            this.isOpen = true;
        }
        else {
            this.isOpen = false;
        }
        */
    }

    // Called every frame if door is opening, slowly opens door every frame
    void open() {
        if (Vector3.Distance(RightDoor.transform.position, LeftDoor.transform.position) < 6) {
            RightDoor.transform.Translate(-(Vector3.forward * this.moveSpeed) * Time.deltaTime);
            LeftDoor.transform.Translate((Vector3.forward * this.moveSpeed) * Time.deltaTime);
        }
    }

    // Called every frame if door is closing, slowly closes door every frame
    void close() {
        if (Vector3.Distance(RightDoor.transform.position, LeftDoor.transform.position) > this.closeDistance) {
            RightDoor.transform.Translate((Vector3.forward * this.moveSpeed) * Time.deltaTime);
            LeftDoor.transform.Translate(-(Vector3.forward * this.moveSpeed) * Time.deltaTime);
        }
    }

    public void startOpening() {
        this.Icon.SetActive(false);
        this.isOpen = true;
    }

    public void startClosing() {
        this.Icon.SetActive(true);
        this.isOpen = false;
    }

    /* TODO
    - Check player vicinity less frequently (minimize lag)
    - Compare player distance to distance of middle of doors (rather than right)
    */

    // Checks if player is close enough to open door
    bool checkPlayerInVicinity() {
        if (Vector3.Distance(RightDoor.transform.position, Player.transform.position) < 5) {
            return true;
        }
        else {
            return false;
        }
    }
}
