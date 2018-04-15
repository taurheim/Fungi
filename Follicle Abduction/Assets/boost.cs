using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boost : MonoBehaviour {

    public bool hit1;
    public bool hit2;
    public bool hit3;

    public GameObject button;
    public GameObject player;
    public GameObject door;

    // Use this for initialization
    void Start () {




    }
	
	// Update is called once per frame
	void Update () {

        if (player == null) 
            player = GameObject.FindGameObjectWithTag("playerA");

        if (button.GetComponentInParent<ButtonPress>().getButtonStatus())
            hit2 = true;

        if (Input.GetKeyDown("f"))
            
            hit3 = true;

        if (hit3 || (hit2&&hit1))
        {
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Rigidbody>().mass = 0;
            player.transform.parent = transform;

            transform.Translate(Vector3.back * 10 * 1 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerA")
        {
            hit1 = true;
            player.GetComponent<Rigidbody>().useGravity = false;
            player.GetComponent<Rigidbody>().mass = 0;
            player.transform.parent = transform;

            transform.Translate(Vector3.back * 10 * 1 * Time.deltaTime);
        }

    }


}
