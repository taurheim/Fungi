using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour {

    Camera mainCamera;

    [SerializeField]
    float speed = 1;

	// Use this for initialization
	void Start ()
    {
        mainCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 angle = Vector3.zero;
        angle.x += Time.deltaTime * speed;
        mainCamera.transform.Rotate(angle);
	}
}
