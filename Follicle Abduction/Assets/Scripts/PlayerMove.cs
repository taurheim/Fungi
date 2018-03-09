using UnityEngine;
using UnityEngine.Networking;

/*
	Used in the network test to handle player movement. 
 */
public class PlayerMove : NetworkedPlayer
{

	void Update ()
	{
		/*
			Once two players have joined, two of this object will exist in the world. isLocalPlayer will be true if
			we have authority over the object. This means that any positional updates to it will be mirrored.

			Try removing this check - it'll mean both clients move both players whenever they use wasd, but only one
			of the movements will be mirrored to the other client (they will de-sync)
		 */
		if (hasAuthority) {
			var x = Input.GetAxis ("Horizontal") * 0.1f;
			var z = Input.GetAxis ("Vertical") * 0.1f;

			transform.Translate (x, 0, z);
		}
	}
}
