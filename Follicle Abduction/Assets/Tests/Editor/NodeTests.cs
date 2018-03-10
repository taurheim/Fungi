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

		parentNode.state = NodeState.LOCKED;

		return parentNode;
	}

	[Test]
	public void UnlockNode() {
		Node parentNode = SetupWithChildren(0);
		Assert.AreEqual(parentNode.state, NodeState.LOCKED);

		parentNode.unlockNode();

		Assert.AreEqual(parentNode.state, NodeState.UNLOCKED);
	}

	[Test]
	public void CompleteNode() {
		Node parentNode = SetupWithChildren(0);
		Assert.AreEqual(parentNode.state, NodeState.LOCKED);

		parentNode.unlockNode();
		parentNode.completeNode();

		Assert.AreEqual(parentNode.state, NodeState.COMPLETED);
	}

	[Test]
	public void CantCompleteLockedNode() {
		Node parentNode = SetupWithChildren(0);
		Assert.AreEqual(parentNode.state, NodeState.LOCKED);
		parentNode.completeNode();
		Assert.AreEqual(parentNode.state, NodeState.LOCKED);
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

	[Test]
	public void SelectNode() {
		Node parentNode = SetupWithChildren(0);
		parentNode.Select();
		Assert.True(parentNode.selected);
	}

	class MockNode : Node {
		public bool wasUsed = false;
		public override void StartAction() {
			wasUsed = true;
		}
	}

	[Test]
	public void UseNode() {
		GameObject parentGameObject = new GameObject();
		MockNode node = parentGameObject.AddComponent<MockNode>();
		node.unlockNode();
		node.completeNode();
		node.useNode();
		Assert.True(node.wasUsed);
	}

	[Test]
	public void CantUseIncompleteNode() {
		GameObject parentGameObject = new GameObject();
		MockNode node = parentGameObject.AddComponent<MockNode>();
		node.useNode();
		Assert.False(node.wasUsed);
	}
}
