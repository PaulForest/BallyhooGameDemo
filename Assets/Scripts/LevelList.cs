using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList
{
    private readonly List<int> _gameLevels = new List<int>();
    public static LevelList Instance => _instance ?? (_instance = new LevelList());
    private static LevelList _instance;

    private static List<int>.Enumerator _gameLevelEnumerator;

    /// <summary>
    /// Dumb workaround to make sure that we keep track of which scenes are levels.
    /// </summary>
    private const int NumberOfScenesThatAreNotGameLevels = 2;

    private LevelList()
    {
        var max = SceneManager.sceneCountInBuildSettings;
        for (var i = NumberOfScenesThatAreNotGameLevels; i < max; i++)
        {
            var scene = SceneManager.GetSceneByBuildIndex(i);

            _gameLevels.Add(i);
            Debug.Log($"Adding a level with build index: {i}");
        }

        if (_gameLevels.Count == 0)
        {
            Debug.LogError($"I need more levels set up in the build settings");
#if UNITY_EDITOR
            Debug.Break();
#endif
        }

        _gameLevelEnumerator = _gameLevels.GetEnumerator();
    }

    public int GetFirstLevelBuildIndex()
    {
        _gameLevelEnumerator = _gameLevels.GetEnumerator();
        _gameLevelEnumerator.MoveNext();

        return _gameLevelEnumerator.Current;
    }

    public int GetNextLevelBuildIndex(int currentLevelBuildIndex)
    {
        if (_gameLevelEnumerator.MoveNext())
        {
            return _gameLevelEnumerator.Current;
        }

        return GetFirstLevelBuildIndex();
    }
}
