using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {

	public string playerA_tag;
	public string playerB_tag;

	private bool setupComplete = false;


	// TODO remove all this crap
	// start crap
	
	PlayerSetup playerA;
	PlayerSetup playerB;

	bool playerRolesSet = false;
	bool playerARoleSet = false;

	GameObject playerA_object;
	GameObject playerA_child;

	GameObject playerB_object;
	GameObject playerB_child;
	
	// end crap

	[SyncVar]
	int connections = 0;

	void Start () 
	{

	}

	void Update () 
	{
		// TODO I'm sure there's a better event-driven way to do this
		// Can we use Awake/Start?
		if(setupComplete) return;

		if (isServer) 
		{

			if (NetworkServer.connections.Count != connections) 
			{
				connections = NetworkServer.connections.Count;		//Track how many connections there are
			}
		}

		// TODO better param setting and loading so we don't have to only compare strings?
		// on the other hand maybe this is the cleanest way to do this
		string role = LevelManager.getParam("playerRole");
		bool isDebug = LevelManager.getParam("isDebug") == "true";

		if(isDebug) {
			Debug.Log("Loading game as " + role);
			GameObject playerAObject = GameObject.FindGameObjectWithTag(playerA_tag);
			GameObject playerBObject = GameObject.FindGameObjectWithTag(playerB_tag);

			// TODO I hate this
			foreach(Behaviour behaviour in playerAObject.GetComponent<Components>().GetComponentList()) {
				behaviour.enabled = role == "0";
			}

			foreach(Behaviour behaviour in playerBObject.GetComponent<Components>().GetComponentList()) {
				behaviour.enabled = role == "1";
			}

			setupComplete = true;
		}
		// TODO remove the rest of this method
		else if (connections == 1 && !playerARoleSet) 
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
		setupComplete = true;
	}

	public int getNumberOfConnections()
	{
		return connections;
	}
}
