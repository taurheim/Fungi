using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	bool playerIsA = false;		// player A - The human
	bool playerIsB = false;		// player B - The alien in the chair

	/*
	[SerializeField]
	GameObject[] playerA_objectsToEnable;

	[SerializeField]
	GameObject[] playerB_objectsToEnable;
	*/

	GameObject playerA_object;
	Behaviour[] playerA_components;

	GameObject playerB_object;
	Behaviour[] playerB_components;

	void Start()
	{

	}

	void Update () 
	{
		
	}

	void enableComponents () 
	{

		if (isLocalPlayer) 
		{
			playerA_object = GameObject.FindGameObjectWithTag("playerA");
			playerA_components = playerA_object.GetComponent<Components> ().GetComponentList();
			playerB_object = GameObject.FindGameObjectWithTag("playerB");
			playerB_components = playerB_object.GetComponent<Components> ().GetComponentList();

			for (int i=0;i<playerA_components.Length;i++){
				playerA_components[i].enabled = playerIsA;
			}

			for (int i=0;i<playerB_components.Length;i++){
				playerB_components[i].enabled = playerIsB;
			}
		}
	}

	public void setPlayerRole(string playerRole)
	{
		if (playerRole.Equals ("playerA")) 
		{
			playerIsA = true;

			//Debug.Log("playerIsA");
		} 
		else if (playerRole.Equals ("playerB")) 
		{
			playerIsB = true;

			//Debug.Log("playerIsB");
		}

		enableComponents ();
	}
}
