using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
public class MenuTests
{
    MenuManager setupMenuManager()
    {
        GameObject obj = new GameObject();
        MenuManager menuManager = obj.AddComponent<MenuManager>();

        menuManager.startScreen = setupStartScreen(menuManager);
        menuManager.hostScreen = setupHostScreen(menuManager);
        menuManager.joinScreen = setupJoinScreen(menuManager);
        menuManager.levelSelect = setupLevelSelect(menuManager);
        menuManager.settingsScreen = setupSettingsScreen(menuManager);

        return menuManager;
    }

    StartScreen setupStartScreen(MenuManager menuManager)
    {
        GameObject obj = new GameObject();
        StartScreen startScreen = obj.AddComponent<StartScreen>();
        startScreen.menuManager = menuManager;
        return startScreen;
    }

    HostScreen setupHostScreen(MenuManager menuManager)
    {
        GameObject obj = new GameObject();
        HostScreen hostScreen = obj.AddComponent<HostScreen>();
        hostScreen.menuManager = menuManager;
        return hostScreen;
    }

    JoinScreen setupJoinScreen(MenuManager menuManager)
    {
        GameObject obj = new GameObject();
        JoinScreen joinScreen = obj.AddComponent<JoinScreen>();
        joinScreen.menuManager = menuManager;
        return joinScreen;
    }


    LevelSelect setupLevelSelect(MenuManager menuManager)
    {
        GameObject obj = new GameObject();
        GameObject drop1 = new GameObject();
        GameObject drop2 = new GameObject();
        LevelSelect levelSelect = obj.AddComponent<LevelSelect>();
        levelSelect.roleSelectLocal = drop1.AddComponent<Dropdown>();
        levelSelect.roleSelectPartner = drop2.AddComponent<Dropdown>();
        levelSelect.menuManager = menuManager;
        return levelSelect;
    }

    SettingsScreen setupSettingsScreen(MenuManager menuManager)
    {
        GameObject obj = new GameObject();
        SettingsScreen settingsScreen = obj.AddComponent<SettingsScreen>();
        settingsScreen.menuManager = menuManager;
        return settingsScreen;
    }

    [Test]
    public void HostGame()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.startScreen.hostButton();
        Assert.True(menuManager.hostScreen.isActiveAndEnabled);
        Assert.False(menuManager.startScreen.isActiveAndEnabled);
    }

    [Test]
    public void JoinGame()
    {

        MenuManager menuManager = setupMenuManager();
        menuManager.startScreen.joinButton();
        Assert.True(menuManager.joinScreen.isActiveAndEnabled);
        Assert.False(menuManager.startScreen.isActiveAndEnabled);
    }

    [Test]
    public void Settings()
    {

        MenuManager menuManager = setupMenuManager();
        menuManager.startScreen.settingsButton();
        Assert.True(menuManager.settingsScreen.isActiveAndEnabled);
        Assert.False(menuManager.startScreen.isActiveAndEnabled);
    }

    [Test]
    public void BackToMainMenu()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.startScreen.hostButton();
        menuManager.hostScreen.backButton();
        Assert.False(menuManager.hostScreen.isActiveAndEnabled);
        Assert.True(menuManager.startScreen.isActiveAndEnabled);
    }

    [Test]
    public void SelectPlayerRole()
    {
        MenuManager menuManager = setupMenuManager();
        bool canSelectRole = menuManager.levelSelect.roleSelectLocal.interactable;
        Assert.True(canSelectRole);
    }

    [Test]
    public void LoadLevel1()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.selectedLevel = 1;
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.True(levelLoads);
    }

    [Test]
    public void LoadLevel2()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.selectedLevel = 2;
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.True(levelLoads);
    }

    [Test]
    public void LoadLevel3()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.selectedLevel = 3;
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.True(levelLoads);
    }

    [Test]
    public void LoadFakeLevel()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.selectedLevel = -1;
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.False(levelLoads);
    }


}
