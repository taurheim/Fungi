using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PatrolTest {

    [Test]
    public void DetectTargetInFront() {
        // Set up our target and guard (patroller) objects
        GameObject target = new GameObject();
        target.transform.position = new Vector3(0f, 0f, 0f);

        // Place guard in front of target, and facing target
        GameObject guard = new GameObject();
        guard.transform.position = new Vector3(1f, 0f, 0f);
        guard.transform.forward = new Vector3(-1f, 0f, 0f);

        // Set up Patrol 
        guard.AddComponent<Patrol>();
        guard.GetComponent<Patrol>().detectRange = 2;
        guard.GetComponent<Patrol>().detectAngle = 90;

        bool detected = guard.GetComponent<Patrol>().Detect(target);

        Object.Destroy(target);
        Object.Destroy(guard);

        Assert.True(detected);
    }

    [Test]
    public void DoNotDetectTargetBehind() {
        // Set up our target and guard (patroller) objects
        GameObject target = new GameObject();
        target.transform.position = new Vector3(0f, 0f, 0f);

        // Place guard in front of target, but facing the other way
        GameObject guard = new GameObject();
        guard.transform.position = new Vector3(1f, 0f, 0f);
        guard.transform.forward = new Vector3(1f, 0f, 0f);

        // Set up Patrol 
        guard.AddComponent<Patrol>();
        guard.GetComponent<Patrol>().detectRange = 2;
        guard.GetComponent<Patrol>().detectAngle = 90;

        bool detected = guard.GetComponent<Patrol>().Detect(target);

        Object.Destroy(target);
        Object.Destroy(guard);

        Assert.False(detected);
    }

    [UnityTest]
    public IEnumerator ResumesPatrolling() {
        Vector3 patrolDestination = new Vector3(10f, 0f, 0f);
        Vector3 origin = new Vector3(0f, 0f, 0f);

        // Create guard at origin
        GameObject guard = new GameObject();
        guard.transform.position = origin;


        // Set up patrol
        guard.AddComponent<Patrol>();
        guard.GetComponent<Patrol>().lastKnownTargetLoc = origin;
        guard.GetComponent<Patrol>().navMesh = new Vector3[] { patrolDestination };

        // Create target
        GameObject target = new GameObject();
        target.transform.position = new Vector3(10f, 10f, 10f);
        guard.GetComponent<Patrol>().detectTargets = new GameObject[] { target };

        // Set up Patrol fields
        guard.GetComponent<Patrol>().detectRange = 0;
        guard.GetComponent<Patrol>().detectAngle = 0;

        yield return null;

        Object.Destroy(target);
        Object.Destroy(guard);

        Assert.True(guard.GetComponent<Patrol>().currDestination == patrolDestination);
    }

    [UnityTest]
    public IEnumerator CapturesTarget() {
        // Set up our target and guard (patroller) objects
        GameObject target = new GameObject();
        target.transform.position = new Vector3(0f, 0f, 0f);

        // Place guard in front of target, and facing target
        GameObject guard = new GameObject();
        guard.transform.position = new Vector3(0.25f, 0f, 0f);
        guard.transform.forward = new Vector3(-1f, 0f, 0f);


        // Set up Patrol 
        guard.AddComponent<Patrol>();
        guard.GetComponent<Patrol>().detectRange = 2;
        guard.GetComponent<Patrol>().detectAngle = 90;
        guard.GetComponent<Patrol>().currChaseTarget = target;

        Vector3 patrolDestination = new Vector3(10f, 0f, 0f);
        guard.GetComponent<Patrol>().navMesh = new Vector3[] { patrolDestination };

        guard.GetComponent<Patrol>().detectTargets = new GameObject[] { target };

        yield return null;

        Object.Destroy(target);
        Object.Destroy(guard);

        Assert.True(guard.GetComponent<Patrol>().currChaseTarget == null);
    }




}
