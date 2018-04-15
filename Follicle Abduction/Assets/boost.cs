using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boost : MonoBehaviour {

    bool hit1;
    bool hit2;

    public GameObject button;
    public GameObject player;

    // Use this for initialization
    void Start () {




    }
	
	// Update is called once per frame
	void Update () {

        if (player == null) 
            player = GameObject.FindGameObjectWithTag("playerA");

        if (button.GetComponentInParent<ButtonPress>().getButtonStatus())
            hit2 = true;



        if (hit1 && hit2)
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
        }

    }
}
