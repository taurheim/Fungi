using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to disable the alien audio listener if player is the human
public class AlienAudio : MonoBehaviour {

    CustomNetworkManager manager;

	void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();
	}

    void Update()
    {
        if (manager.myRole == "human")
        {
            gameObject.SetActive(false);
        }
        else
        {
            GameObject playerA = GameObject.FindGameObjectWithTag("playerA");
            if(playerA != null) {
                AudioListener listener = playerA.GetComponent<AudioListener>();
                listener.enabled = false;
            }
        }
    }
	
}
