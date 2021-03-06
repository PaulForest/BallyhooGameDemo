﻿using System.Collections.Generic;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList
{
    /// <summary>
    ///     Dumb workaround to make sure that we keep track of which scenes are levels.
    /// </summary>
    private const int NumberOfScenesThatAreNotGameLevels = 1;

    private static LevelList _instance;

    private static List<LevelData>.Enumerator _gameLevelEnumerator;
    private readonly List<LevelData> _gameLevels = new List<LevelData>();

    private LevelList()
    {
        var max = SceneManager.sceneCountInBuildSettings;
        for (var i = NumberOfScenesThatAreNotGameLevels; i < max; i++)
            _gameLevels.Add(new LevelData {buildIndex = i, isBoss = false});

        if (_gameLevels.Count == 0)
        {
            Debug.LogError("I need more levels set up in the build settings");
#if UNITY_EDITOR
            Debug.Break();
#endif
        }

        _gameLevelEnumerator = _gameLevels.GetEnumerator();
    }

    public static LevelList Instance => _instance ?? (_instance = new LevelList());

    public LevelData GetFirstLevelData()
    {
        _gameLevelEnumerator = _gameLevels.GetEnumerator();
        _gameLevelEnumerator.MoveNext();

        return _gameLevelEnumerator.Current;
    }

    public LevelData GetNextLevelData()
    {
        if (_gameLevelEnumerator.MoveNext()) return _gameLevelEnumerator.Current;

        return GetFirstLevelData();
    }

    public LevelData GetLevelDataForBuildIndex(int buildIndex)
    {
        var levelData = _gameLevels.Find(data => data.buildIndex == buildIndex);

        if (null != levelData) return levelData;

        Debug.LogError(
            $"GetLevelDataForBuildIndex(buildIndex={buildIndex}): cannot find such a level.  _gameLevels={_gameLevels}");
        levelData = GetFirstLevelData();

        return levelData;
    }
}