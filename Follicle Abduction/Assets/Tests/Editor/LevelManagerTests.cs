
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManagerTests {
  [Test]
  public void GetFirstLevel() {
    LevelManager.resetState();
    string[] allLevels = LevelManager.getLevelList();
    Assert.AreEqual(LevelManager.getCurrentLevel(), allLevels[0]);
  }

  [Test]
  public void CompleteLevel() {
    LevelManager.resetState();
    string currentLevel = LevelManager.getCurrentLevel();
    LevelManager.goToNextLevel();
    Assert.AreNotEqual(currentLevel, LevelManager.getCurrentLevel());
  }

  [Test]
  public void CompleteGame() {
    LevelManager.resetState();
    int levelCount = LevelManager.getLevelList().Length;
    for (int i=0; i < levelCount; i++) {
      LevelManager.goToNextLevel();
    }

    Assert.True(LevelManager.isGameComplete());

  }

  [Test]
  public void UnlockedLevel() {
    LevelManager.resetState();
    Assert.False(LevelManager.isLevelAvailable("level_2"));
    LevelManager.goToNextLevel();
    Assert.True(LevelManager.isLevelAvailable("level_2"));
  }
}