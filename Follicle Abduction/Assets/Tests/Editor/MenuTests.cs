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
    public void ChangeSettings()
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
        menuManager.levelSelect.selectedLevel = "Level_1";
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.True(levelLoads);
    }

    [Test]
    public void LoadLevel2()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.selectedLevel = "Level_2";
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.True(levelLoads);
    }

    [Test]
    public void LoadLevel3()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.selectedLevel = "Level_3";
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.True(levelLoads);
    }

    [Test]
    public void LoadFakeLevel()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.selectedLevel = "";
        bool levelLoads = menuManager.levelSelect.loadScene();
        Assert.False(levelLoads);
    }

    [Test]
    public void EnterPort()
    {
        MenuManager menuManager = setupMenuManager();
        int port = 7777;
        menuManager.joinScreen.setPort(port);

        Assert.True(port == menuManager.joinScreen.getPort());
    }

    [Test]
    public void EnterIP()
    {
        MenuManager menuManager = setupMenuManager();
        string ip = "localhost";
        menuManager.joinScreen.setIP(ip);

        Assert.True(ip == menuManager.joinScreen.getIP());
    }

    [Test]
    public void GameBeginsAfterRolesChosen()
    {
        MenuManager menuManager = setupMenuManager();
        menuManager.levelSelect.localPlayerReady = true;
        menuManager.levelSelect.partnerPlayerReady = false;
        menuManager.levelSelect.startGame();
        Assert.True(menuManager.levelSelect.gameStarted);
    }

    [Test]
    public void LevelsInOrder()
    {
        MenuManager menuManager = setupMenuManager();
        bool lvl1 = menuManager.levelSelect.lvl1_unlocked;
        bool lvl2 = menuManager.levelSelect.lvl2_unlocked;
        bool lvl3 = menuManager.levelSelect.lvl3_unlocked;

        Assert.True(lvl1);
        Assert.False(lvl2);
        Assert.False(lvl3);
    }

}
