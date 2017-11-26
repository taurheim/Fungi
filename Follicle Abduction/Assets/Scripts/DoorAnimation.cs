using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour {

    Animation animations;

    [SerializeField]
    bool startOpen = false;

    bool isOpen = false;

    void Start ()
    {
        animations = GetComponent<Animation>();

        // This is just incase the door need to be open at the start
        if (startOpen)
        {
            instantOpen();
        }

        //close();
    }
	
	public void open()
    {
        if (!isOpen)
        {
            animations.Play("open", PlayMode.StopAll);
            isOpen = true;
        }
    }

    public void close()
    {
        if (isOpen)
        {
            animations.Play("close", PlayMode.StopAll);
            isOpen = false;
        }
    }

    void instantOpen()
    {
        animations.Play("instantOpen", PlayMode.StopAll);
        isOpen = true;     
    }
}
