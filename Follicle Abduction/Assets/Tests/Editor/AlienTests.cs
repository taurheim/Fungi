using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AlienTests {

	AlienPlayer alien;

	[SetUp]
	public void SetUp() {
		//Map visibility is on by default
		alien = new AlienPlayer ();
	}

	[Test]
	public void MoveTests() {
		Node parentNode = new Node();
		Node rightNode = new Node();
		Node upNode = new Node();
		Node downNode = new Node();
		Node leftNode = new Node();
		parentNode.childNodes[0] = upNode;
		parentNode.childNodes[1] = leftNode;
		parentNode.childNodes[2] = downNode;
		parentNode.childNodes[3] = rightNode;
		alien.currentNode = parentNode;
		parentNode.isSelected = true;

		// Move between all the nodes and make sure each gets selected as we go through it
		alien.moveRight();
		Assert.Equals(alien.currentNode, rightNode);
		Assert.True(alien.currentNode.isSelected);
		alien.moveLeft();
		Assert.Equals(alien.currentNode, parentNode);
		Assert.True(alien.currentNode.isSelected);
		alien.moveLeft();
		Assert.Equals(alien.currentNode, leftNode);
		Assert.True(alien.currentNode.isSelected);
		alien.moveRight();
		Assert.Equals(alien.currentNode, parentNode);
		Assert.True(alien.currentNode.isSelected);
		alien.moveUp();
		Assert.Equals(alien.currentNode, upNode);
		Assert.True(alien.currentNode.isSelected);
		alien.moveDown();
		Assert.Equals(alien.currentNode, parentNode);
		Assert.True(alien.currentNode.isSelected);
		alien.moveDown();
		Assert.Equals(alien.currentNode, downNode);
		Assert.True(alien.currentNode.isSelected);
	}
		
}
