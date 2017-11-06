using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {

	PlayerSetup playerA;
	PlayerSetup playerB;

	bool playerRolesSet = false;
	bool playerARoleSet = false;

	GameObject playerA_object;
	GameObject playerA_child;

	GameObject playerB_object;
	GameObject playerB_child;

	[SyncVar]
	int connections = 0;

	void Start () 
	{

	}

	void Update () 
	{
		if (isServer) 
		{

			if (NetworkServer.connections.Count != connections) 
			{
				connections = NetworkServer.connections.Count;		//Track how many connections there are

			}
		}


		//This is a mess but it (almost)works for now
		if (connections == 1 && !playerARoleSet) 
		{
			if (GameObject.FindGameObjectsWithTag ("Player").Length == 1) 	//one player object exists
			{
				playerA = GameObject.FindGameObjectsWithTag ("Player") [0].GetComponent<PlayerSetup> ();
				playerA.setPlayerRole ("playerA");

				playerARoleSet = true;
			}
		}

		if (connections == 2 && !playerRolesSet) 
		{
			if (!playerARoleSet) 
			{
				setPlayerRoles ();
			} 
			else 
			{
				if (GameObject.FindGameObjectsWithTag ("Player").Length == 2) 	//Both player objects exist
				{
					playerB = GameObject.FindGameObjectsWithTag ("Player") [1].GetComponent<PlayerSetup> ();
					playerB.setPlayerRole ("playerB");

					playerRolesSet = true;
				}
			}
		}
		
	}

	public void setPlayerRoles()
	{
		if (GameObject.FindGameObjectsWithTag ("Player").Length == 2) 	//Both player objects exist
		{
			playerA = GameObject.FindGameObjectsWithTag ("Player") [0].GetComponent<PlayerSetup> ();
			playerA.setPlayerRole ("playerA");

			playerB = GameObject.FindGameObjectsWithTag ("Player") [1].GetComponent<PlayerSetup> ();
			playerB.setPlayerRole ("playerB");

			playerRolesSet = true;
			playerARoleSet = true;
		}
	}

	public int getNumberOfConnections()
	{
		return connections;
	}
}
