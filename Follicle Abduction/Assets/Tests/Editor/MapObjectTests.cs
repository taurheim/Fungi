using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapObjectTests {

	MapObject obj;

	[SetUp]
	public void SetUp() {
		obj = new MapObject();
	}

	[Test]
	public void SetMapIcon() {
		GameObject mapIcon = GameObject.CreatePrimitive (PrimitiveType.Cube);
		obj.setMapIcon (mapIcon);
		Assert.AreEqual (obj.mapIcon, mapIcon);
	}

	[Test]
	public void GetVisibility() {
		GameObject mapIcon = GameObject.CreatePrimitive (PrimitiveType.Cube);
		mapIcon.GetComponent<Renderer> ().enabled = true;
		obj.setMapIcon (mapIcon);
		Assert.True (obj.isVisible ());
		mapIcon.GetComponent<Renderer> ().enabled = false;
		Assert.False (obj.isVisible ());
	}

	[Test]
	public void SetVisibility() {
		GameObject mapIcon = GameObject.CreatePrimitive (PrimitiveType.Cube);
		mapIcon.GetComponent<Renderer> ().enabled = true;
		obj.setMapIcon (mapIcon);
		Assert.True (obj.isVisible ());
		obj.setVisibility (false);
		Assert.False (obj.isVisible ());
		obj.setVisibility (true);
		Assert.True (obj.isVisible ());
	}

	[Test]
	public void GetVisibilityWithoutIconNoError() {
		Assert.True (obj.mapIcon == null);
		obj.isVisible ();
	}

	[Test]
	public void SetVisibilityWithoutIconNoError() {
		Assert.True (obj.mapIcon == null);
		obj.setVisibility (true);
	}


}
