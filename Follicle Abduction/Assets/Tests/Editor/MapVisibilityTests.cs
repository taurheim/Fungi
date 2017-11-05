using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MapVisibilityTests {

	public MapObject SetUpMapObject(string tag, bool visible){
		GameObject parentGameObj = new GameObject();
		MapObject mapObj = parentGameObj.AddComponent<MapObject>();
		mapObj.setMapIcon (GameObject.CreatePrimitive(PrimitiveType.Cube));
		mapObj.setVisibility (visible);
		mapObj.tag = tag;
		return mapObj;
	}
		
	[Test]
	public void TurnsOffMapVisibilityForGivenTag() {
		MapObject obj1 = SetUpMapObject ("dontToggle", true);
		MapObject obj2 = SetUpMapObject ("dontToggle", true);
		MapObject obj3 = SetUpMapObject ("doToggle", true);
		MapObject obj4 = SetUpMapObject ("doToggle", true);

		MapVisibility.setVisibilityForTag ("doToggle", false);

		Assert.True (obj1.isVisible ());
		Assert.True (obj2.isVisible ());
		Assert.False (obj3.isVisible ());
		Assert.False (obj4.isVisible ());
	}

	[Test]
	public void TurnsOnMapVisibilityForGivenTag() {
		MapObject obj1 = SetUpMapObject ("dontToggle", false);
		MapObject obj2 = SetUpMapObject ("dontToggle", false);
		MapObject obj3 = SetUpMapObject ("doToggle", false);
		MapObject obj4 = SetUpMapObject ("doToggle", false);

		MapVisibility.setVisibilityForTag ("doToggle", true);

		Assert.False (obj1.isVisible ());
		Assert.False (obj2.isVisible ());
		Assert.True (obj3.isVisible ());
		Assert.True (obj4.isVisible ());
	}

}
