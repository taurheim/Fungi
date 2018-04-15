using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boost : MonoBehaviour {

    bool hit1;
    bool hit2;

    public GameObject button;

    // Use this for initialization
    void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {
        if (button.GetComponentInParent<ButtonPress>().getButtonStatus())
            hit2 = true;


        if (hit2)
            transform.Translate(Vector3.back * 10 * 1 * Time.deltaTime);






    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerA")
        {
            hit1 = true;
            other.transform.parent = transform;
            other.GetComponent<Rigidbody>().useGravity.Equals(false);
        }

    }
}
