using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RoleSelect : NetworkBehaviour {

    public Dropdown host;
    public int hostValue;

    public Dropdown client;
    public int clientValue;

	// Use this for initialization
	void Start ()
    {
        hostValue = host.value;
        clientValue = client.value;
    }
	
	// Update is called once per frame
	void Update ()
    {
       if(host.value != hostValue)  //if the dropdown value had been changed
        {
            //CmdSetHost(host.value);
            hostValue = host.value;
            //network sync
        }
       if(client.value != clientValue)
        {
            clientValue = client.value;
            //network sync
        }
	}

    [ClientRpc]
    public void RpcSetHost(int val)
    {
        host.value = val;
    }

    [Command]
    public void CmdSetHost(int val)
    {
        RpcSetHost(val);
    }
}
