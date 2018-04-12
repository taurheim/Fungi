using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Intro : MonoBehaviour {

    public VideoPlayer vp;

	float time = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		time += Time.deltaTime;

		if(!vp.isPlaying && time > 5f)
        {
            SceneManager.LoadScene("MainMenu");
        }

        if(Input.anyKey)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
