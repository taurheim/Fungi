using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

/*
    Custom implementation of the NetworkManager interface. Currently used mainly as a debug tool.
 */
public class CustomNetworkManager : NetworkManager
{

	public GameMenu menu;

	private bool isDebug;

	// Called when someone joins the unity server
	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		if (conn.hostId == 0) {
			menu.NotifyPlayerAdded (conn, playerControllerId);
		}
	}
}