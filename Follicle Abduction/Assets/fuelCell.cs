using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fuelCell : MonoBehaviour {

    public int testing = 2;
    public GameObject cell1;
    public GameObject cell2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "playerA")
        {
            if(cell1.active == false)
                cell2.transform.position = new Vector3(8, -11.295f, 40.302f);
                
        }

    }
}
