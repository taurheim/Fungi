using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : NetworkedObject {

    Image image;
    CustomNetworkManager manager;

	public override void Start ()
    {
        image = GetComponent<Image>();
        manager = GameObject.FindGameObjectWithTag("networkmanager").GetComponent<CustomNetworkManager>();
    }

    public void gameOver()
    {
        NetworkInteract();

        if (manager.isHost)
        {
            StartCoroutine(waitBeforeRestart());
        }
    }

    IEnumerator waitBeforeRestart()
    {
        yield return new WaitForSeconds(3);

        manager.NetworkLoadScene(gameObject.scene.name);
    }

    protected override void Interact()
    {
        image.enabled = true;
    }
}
