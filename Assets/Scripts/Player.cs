﻿using Level;
using UnityEngine;

public class Player
{
    private const string LastLevelKey = "LastLevelPlayed";
    private static Player _instance;

    public static Player Instance => _instance ?? (_instance = CreateNewOrGetExistingPlayer());

    public LevelData LastLevelPlayed { get; set; }

    private static bool IsNewPlayer()
    {
        return PlayerPrefs.HasKey(LastLevelKey);
    }

    private static Player CreateNewOrGetExistingPlayer()
    {
        return IsNewPlayer() ? CreateNewPlayer() : CreatePlayerFromData();
    }

    private static Player CreateNewPlayer()
    {
        var player = new Player
        {
            LastLevelPlayed = LevelList.Instance.GetFirstLevelData()
        };
        return player;
    }

    private static Player CreatePlayerFromData()
    {
        var player = new Player
        {
            LastLevelPlayed = LevelList.Instance.GetLevelDataForBuildIndex(PlayerPrefs.GetInt(LastLevelKey))
        };
        return player;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(LastLevelKey, LastLevelPlayed.buildIndex);
        PlayerPrefs.Save();
    }
}
