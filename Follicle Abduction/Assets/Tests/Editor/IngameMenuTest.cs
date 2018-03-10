using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenuTest {

	[Test]
	public void showMenu() {
        GameObject InGameMenu = new GameObject();
        Transform canvas = InGameMenu.AddComponent<Transform>();
        IngameMenu igmScript = InGameMenu.AddComponent<IngameMenu>();

        canvas.gameObject.SetActive(true);

        igmScript.ViewMenu();

        Assert.True(canvas.gameObject.activeInHierarchy);
    }

    [Test]
    public void hideMenu() {
        GameObject InGameMenu = new GameObject();
        Transform canvas = InGameMenu.AddComponent<Transform>();
        IngameMenu igmScript = InGameMenu.AddComponent<IngameMenu>();

        canvas.gameObject.SetActive(false);

        igmScript.ViewMenu();

        Assert.False(canvas.gameObject.activeInHierarchy);
    }

    [Test]
    public void returnMainMenu() {
        GameObject InGameMenu = new GameObject();
        Transform canvas = InGameMenu.AddComponent<Transform>();
        IngameMenu igmScript = InGameMenu.AddComponent<IngameMenu>();

        igmScript.ReturnToMainMenu();

        Scene scene = SceneManager.GetActiveScene();

        Assert.True(scene.name == "MainMenu");
    }

}
