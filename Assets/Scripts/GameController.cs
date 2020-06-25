﻿using System.Collections;
using Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

[RequireComponent((typeof(DontDestroyOnLoad)))]
public class GameController : MonoBehaviour
{
    public static GameController Instance
    {
        get
        {
            if (_instance) return _instance;


            Debug.LogError($"GameController should be in the scene");
            return null;

            // var go = new GameObject("GameController");
            // _instance = go.AddComponent<GameController>();
            // return _instance;
        }
    }

    private static GameController _instance;

    private void Awake()
    {
        if (_instance && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        ResetAllStaticData.Reset();
    }

    private void Start()
    {
        GlobalEvents.LevelWon.AddListener(OnLevelWon);
        GlobalEvents.LevelLost.AddListener(OnLevelLost);

        StartLevel(Player.Instance.LastLevelPlayed);
    }

    private void StartLevel(LevelData levelData)
    {
        StartCoroutine(StartLevelCoroutine(levelData));
    }

    private IEnumerator StartLevelCoroutine(LevelData levelData)
    {
        ResetAllStaticData.Reset();

        yield return SceneManager.LoadSceneAsync(levelData.buildIndex, LoadSceneMode.Additive);

        var player = Player.Instance;

        LevelController.ResetInstance();

        GlobalEvents.LevelStart?.Invoke(levelData);
    }

    private void OnLevelWon(LevelData levelData)
    {
        StartCoroutine(OnLevelWonCoroutine());
    }

    private IEnumerator OnLevelWonCoroutine()
    {
        ResetAllStaticData.Reset();
        LevelController.Instance.HaltExecution();

        var player = Player.Instance;

        yield return SceneManager.UnloadSceneAsync(player.LastLevelPlayed.buildIndex);

        player.LastLevelPlayed = LevelList.Instance.GetNextLevelData();
        player.SaveData();

        StartLevel(player.LastLevelPlayed);
    }

    private void OnLevelLost(LevelData levelData)
    {
        Debug.Log("GameController.OnLevelLost()");
        StartCoroutine(OnLevelLostCoroutine());
    }

    private IEnumerator OnLevelLostCoroutine()
    {
        ResetAllStaticData.Reset();
        LevelController.Instance.HaltExecution();


        yield return new WaitForSeconds(1);

        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        StartLevel(Player.Instance.LastLevelPlayed);
    }
}
