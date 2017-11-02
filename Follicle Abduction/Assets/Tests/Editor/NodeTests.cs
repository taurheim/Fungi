using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnitTestTest {

	[Test]
	public void UnitTestTestSimplePasses() {
		// Use the Assert class to test conditions.
		Assert.True(true);
	}

	[Test]
	public void Test_CustomNodeTest() {
		Assert.False(false);
	}
}
