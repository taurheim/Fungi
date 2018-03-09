using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Handle suspiciousness
	TODO: Still a stub, build out once suspiciousness is fully implemented
 */
public class Alien : MonoBehaviour
{

	public int maxSuspiciousness = 100;
	private int suspiciousness;
	private bool captured;

	void Start ()
	{
		suspiciousness = 0;
		captured = false;
	}

	public void doSomethingSuspicious (int howSuspicious)
	{
		suspiciousness += howSuspicious;
		if (suspiciousness >= maxSuspiciousness) {
			captured = true;
		}
	}

	public int getSuspiciousness ()
	{
		return suspiciousness;
	}

	public bool hasBeenCaptured ()
	{
		return captured;
	}
}
