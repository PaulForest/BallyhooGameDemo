using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private readonly Dictionary<string, Scene> _gameLevels = new Dictionary<string, Scene>();
    public Player Player { get; private set; }

    public static GameController Instance => _instance;

    private void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;

        var max = SceneManager.sceneCountInBuildSettings;
        for (var i = 0; i < max; i++)
        {
            var scene = SceneManager.GetSceneByBuildIndex(i);
            if (scene.name.ToLower().Contains("level") || scene.path.ToLower().Contains("level"))
            {
                _instance._gameLevels.Add(scene.name, scene);
                Debug.Log($"{scene.path}");
            }
        }
    }

    private static GameController _instance;

    private void Start()
    {
        Player = Player.Instance;
        if (string.IsNullOrEmpty(Player.LastLevelPlayed))
        {
            // Player.LastLevelPlayed = _gameLevels.;
        }
    }
}
