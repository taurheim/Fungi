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
        canvas = InGameMenu.GetComponent<Transform>();
        igmScript = InGameMenu.AddComponent<IngameMenu>();

        igmScript.canvas = canvas;
    }


    [Test]
    public void showMenu() {
        canvas.gameObject.SetActive(false);
        igmScript.ViewMenu();

        Assert.True(canvas.gameObject.activeInHierarchy);
    }

    [Test]
    public void hideMenu() {
        canvas.gameObject.SetActive(true);
        igmScript.ViewMenu();

        Assert.False(canvas.gameObject.activeInHierarchy);
    }

    [Test]
    public void returnMainMenu() {
        igmScript.ReturnToMainMenu();

        Assert.True(SceneManager.sceneCount > 1);
    }

    [Test]
    public void pause() {
        igmScript.Pause();

        Assert.True(Time.timeScale == 0);
    }

    [Test]
    public void unpause() {
        igmScript.Unpause();

        Assert.True(Time.timeScale == 1);
    }

}
