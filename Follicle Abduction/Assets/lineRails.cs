using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineRails : MonoBehaviour {

    private float speed = 1;

    public int direction = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * speed * direction * Time.deltaTime);
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "playerA")
        {
            other.transform.parent = transform;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "playerA")
        {
            other.transform.parent = null;
        }

    }

}
