using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
    Custom game menu. Handles networking & connections.
    This screen shows up after a level is selected, and acts as a lobby while the other player connects.
 */
public class GameMenu : MonoBehaviour
{
	bool gamePaused = false;

	public void pauseGame ()
	{
		//TODO Pausing the game for real
		gamePaused = true;
	}

	public bool isGamePaused ()
	{
		return gamePaused;
	}

}