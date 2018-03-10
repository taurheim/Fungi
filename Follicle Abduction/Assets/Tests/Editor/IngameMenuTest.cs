using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenuTest {
    GameObject InGameMenu;
    Transform canvas;
    IngameMenu igmScript;

    [SetUp]
    public void SetUp() {
        InGameMenu = new GameObject();
        canvas = InGameMenu.AddComponent<Transform>();
        igmScript = InGameMenu.AddComponent<IngameMenu>();
    }


    [Test]
	public void showMenu() {
        canvas.gameObject.SetActive(true);
        igmScript.ViewMenu();

        Assert.True(canvas.gameObject.activeInHierarchy);
    }

    [Test]
    public void hideMenu() {
        canvas.gameObject.SetActive(false);
        igmScript.ViewMenu();

        Assert.False(canvas.gameObject.activeInHierarchy);
    }

    [Test]
    public void returnMainMenu() {
        igmScript.ReturnToMainMenu();
        Scene scene = SceneManager.GetActiveScene();

        Assert.True(scene.name == "MainMenu");
    }

}
