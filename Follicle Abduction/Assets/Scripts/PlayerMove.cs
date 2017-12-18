using UnityEngine;
using UnityEngine.Networking;

/*
	Used in the network test to handle player movement. 
 */
public class PlayerMove : NetworkBehaviour
{
    void Update()
    {
		/*
		if(isServer && NetworkServer.connections.Count > 1 && NetworkServer.connections[1] != null){
			NetworkServer.AddPlayerForConnection(NetworkServer.connections[1], gameObject, 0);
		}*/
		if(!isServer) {
			var x = Input.GetAxis("Horizontal")*0.1f;
			var z = Input.GetAxis("Vertical")*0.1f;

			transform.Translate(x, 0, z);
		}
    }
}