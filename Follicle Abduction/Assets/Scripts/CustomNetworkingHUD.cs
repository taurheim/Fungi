using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
	Custom implementation of the NetworkManagerHUD. Mostly just an import of the Unity source for
	NetworkManagerHUD, but this will eventually build the HUD that Follicle Abduction uses.

	NOTE: Most of this code is uncommented because it is a direct copy from the Unity source from:
	https://forum.unity.com/threads/networkmanagerhud-source.333482/
 */

[AddComponentMenu ("Network/NetworkManagerHUD")]
[RequireComponent (typeof(NetworkManager))]
public class CustomNetworkingHUD : MonoBehaviour
{

	public CustomNetworkManager manager;
	public bool showGUI;
	public int offsetX;
	public int offsetY;

	bool showServer = false;

	bool debugStarted = false;

	void Awake ()
	{
		manager = GetComponent<CustomNetworkManager> ();
	}

	void Update ()
	{
	}

	void OnGUI ()
	{
		// Start the game if we're in debug mode
		bool isDebug = LevelManager.getParam ("isDebug") == "True";
		if (isDebug || !showGUI)
			return;

		int xpos = 10 + offsetX;
		int ypos = 40 + offsetY;
		int spacing = 24;

		if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null) {
			if (GUI.Button (new Rect (xpos, ypos, 200, 20), "Debug as Alien")) {
				manager.debugAsRole("alien");
			}
			ypos += spacing;

			if (GUI.Button (new Rect (xpos, ypos, 200, 20), "Debug as Human")) {
				manager.debugAsRole("human");
			}
		}
	}
}
