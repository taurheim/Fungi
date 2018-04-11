using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro : MonoBehaviour {

    public VideoPlayer vp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(!vp.isPlaying)
        {
            SceneManager.LoadScene("MainMenu");
        }

        if(Input.anyKey)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
