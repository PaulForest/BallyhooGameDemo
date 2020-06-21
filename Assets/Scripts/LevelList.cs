using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelList
{
    private readonly List<string> _gameLevels = new List<string>();
    public static LevelList Instance => _instance ?? (_instance = new LevelList());
    private static List<string>.Enumerator _gameLevelEnumerator;
    private static LevelList _instance;

    private LevelList()
    {
        var max = SceneManager.sceneCountInBuildSettings;
        for (var i = 0; i < max; i++)
        {
            var scene = SceneManager.GetSceneByBuildIndex(i);
            if (!scene.name.ToLower().Contains("level") && !scene.path.ToLower().Contains("level")) continue;

            _instance._gameLevels.Add(scene.name);
            Debug.Log($"{scene.path}");
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

    public string GetFirstLevelName()
    {
        return _gameLevels[0];
    }

    public string GetNextLevelName(string currentLevelName)
    {
        var index = _gameLevels.IndexOf(currentLevelName);
        if (index == _gameLevels.Count - 1)
        {
            index = 0;
        }

        return _gameLevels[index];
    }
}
