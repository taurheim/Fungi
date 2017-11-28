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
	public bool isDebug;
	
	// end crap

	[SyncVar]
	int connections = 0;

	void Start () 
	{
	}

	void Update () 
	{
		return;
		Debug.Log(LevelManager.getParam("isDebug"));
		isDebug = LevelManager.getParam("isDebug") == "True";
		// TODO I'm sure there's a better event-driven way to do this
		// Can we use Awake/Start?
		if(setupComplete) return;

		Debug.Log(NetworkServer.connections.Count + " connected.");

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

		if(isDebug) {
			Debug.Log("We're in debug mode - don't wait for more connections");
			setupGameForRole(role);
			setupComplete = true;
		}
		else if (connections == 2) 
		{
			Debug.Log("Two players connected! Start game now!");
			if(isServer){
				Debug.Log("I'm the alien!");
				setupGameForRole("1");
			} else {
				Debug.Log("I'm the human!");
				setupGameForRole("0");
			}
			setupComplete = true;
		}
	}

	void setupGameForRole(string role){
		GameObject playerAObject = GameObject.FindGameObjectWithTag(playerA_tag);
		GameObject playerBObject = GameObject.FindGameObjectWithTag(playerB_tag);

		// TODO I hate this
		foreach(Behaviour behaviour in playerAObject.GetComponent<Components>().GetComponentList()) {
			behaviour.enabled = role == "0";
		}

		foreach(Behaviour behaviour in playerBObject.GetComponent<Components>().GetComponentList()) {
			behaviour.enabled = role == "1";
		}
	}

	public int getNumberOfConnections()
	{
		return connections;
	}
}
