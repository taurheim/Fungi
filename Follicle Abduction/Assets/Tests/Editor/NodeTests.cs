using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;

public class NodeTests {

	public Node SetupWithChildren(int children){
		GameObject parentGameObject = new GameObject();
		Node parentNode = parentGameObject.AddComponent<Node>();
		Node[] childNodes = new Node[children];
		for(int i=0;i<children;i++){
			childNodes[i] = Substitute.For<Node>();
		}
		parentNode.childNodes = childNodes;

		parentNode.isUnlocked = false;
		parentNode.isCompleted = false;
		return parentNode;
	}

	[Test]
	public void UnlockNode() {
		Node parentNode = SetupWithChildren(0);
		Assert.False(parentNode.isUnlocked);
		Assert.False(parentNode.isCompleted);

		parentNode.unlockNode();

		Assert.True(parentNode.isUnlocked);
		Assert.False(parentNode.isCompleted);
	}

	[Test]
	public void CompleteNode() {
		Node parentNode = SetupWithChildren(0);
		Assert.False(parentNode.isUnlocked);
		Assert.False(parentNode.isCompleted);

		parentNode.unlockNode();
		parentNode.completeNode();

		Assert.True(parentNode.isUnlocked);
		Assert.True(parentNode.isCompleted);
	}

	[Test]
	public void CantCompleteLockedNode() {
		Node parentNode = SetupWithChildren(0);
		parentNode.completeNode();

		Assert.False(parentNode.isCompleted);
		Assert.False(parentNode.isUnlocked);
	}

	[Test]
	public void UnlockChildrenOnCompletion() {
		Node parentNode = SetupWithChildren(2);
		parentNode.unlockNode();
		parentNode.completeNode();

		foreach(Node childNode in parentNode.childNodes) {
			childNode.Received().unlockNode();
		}
	}

	[Test]
	public void UnlockNoChildrenWithoutError() {
		Node parentNode = SetupWithChildren(0);
		parentNode.unlockNode();
		parentNode.completeNode();
	}
}
