using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MenuTests
{


    public MainMenu SetupMainMenu()
    {
        GameObject obj = new GameObject();
        MainMenu mainMenu = obj.AddComponent<MainMenu>();
        return mainMenu;
    }

    public GameMenu SetupGameMenu()
    {
        GameObject obj = new GameObject();
        GameMenu gameMenu = obj.AddComponent<GameMenu>();
        return gameMenu;
    }

    [Test]
    public void DemoLevelLoad()
    {
        MainMenu menu = SetupMainMenu();

        bool sceneWillLoad = menu.loadScene("Demo_level");

        Assert.True(sceneWillLoad);
    }

    [Test]
    public void LevelOneLoad()
    {
        MainMenu menu = SetupMainMenu();

        bool sceneWillLoad = menu.loadScene("Level1");

        Assert.True(sceneWillLoad);
    }

    [Test]
    public void LevelTwoLoad()
    {
        MainMenu menu = SetupMainMenu();

        bool sceneWillLoad = menu.loadScene("Level2");

        Assert.True(sceneWillLoad);
    }

    [Test]
    public void PauseGame()
    {
        GameMenu gameMenu = SetupGameMenu();

        gameMenu.pauseGame();

        Assert.True(gameMenu.isGamePaused());
    }

    [Test]
    public void SelectPlayerRole()
    {
        MainMenu menu = SetupMainMenu();

        string playerRole = "Human";
        menu.setPlayerRole(playerRole);

        Assert.True(playerRole.Equals(menu.getPlayerRole()));
    }

    [Test]
    public void changeSettings()
    {
        MainMenu menu = SetupMainMenu();

        menu.updateSettings();

        Assert.True(menu.settingsUpdatedSuccessfully());
    }

}
