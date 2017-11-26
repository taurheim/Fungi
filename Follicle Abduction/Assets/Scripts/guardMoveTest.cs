using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guardMoveTest : MonoBehaviour {

    public float walkSpeed;
    public float runSpeed;

    private Transform guard;

    private Animation animations;

    bool running = false;
    bool walking = false;
    bool looking = false;
    public bool startlookingAround;
    public bool startRunning;

    void Start ()
    {
        guard = GetComponent<Transform>();
        animations = GetComponent<Animation>();

        if (startlookingAround)
        {
            lookAround();
        }
        else if (startRunning)
        {
            runForward();
        }
        else
        {
            walkForward();
        }

    }

    void Update ()
    {
        if (walking)
        {
            Vector3 forward = walkSpeed * guard.forward * Time.deltaTime;
            guard.Translate(forward, Space.World);
        }
        else if (running)
        {
            Vector3 forward = runSpeed * guard.forward * Time.deltaTime;
            guard.Translate(forward, Space.World);
        }
    }

    void walkForward()
    {
        animations.Play("walk_cycle", PlayMode.StopAll);

        walking = true;
    }

    void runForward()
    {
        animations.Play("run_cycle", PlayMode.StopAll);

        running = true;
    }

    void lookAround()
    {
        animations.Play("look_around", PlayMode.StopAll);

        looking = true;
    }
}
