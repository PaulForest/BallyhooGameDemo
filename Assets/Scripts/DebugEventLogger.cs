using UnityEngine;

public class DebugEventLogger : MonoBehaviour
{
#if UNITY_EDITOR

    [Header("Which events should we log?")] [SerializeField]
    private bool levelStart;

    [SerializeField] private bool levelWon;
    [SerializeField] private bool levelLost;
    [SerializeField] private bool ballDestroyed;
    [SerializeField] private bool ballSplit;

    private bool _levelStartSubscribed;
    private bool _levelWonSubscribed;
    private bool _levelLostSubscribed;
    private bool _ballDestroyedSubscribed;
    private bool _ballSplitSubscribed;

    private void OnValidate()
    {
        if (levelStart && !_levelStartSubscribed)
        {
            GlobalEvents.LevelStart.AddListener(LevelStartFunc);
            _levelStartSubscribed = true;
        }
        else if (!levelStart && _levelStartSubscribed)
        {
            GlobalEvents.LevelStart.RemoveListener(LevelStartFunc);
            _levelStartSubscribed = false;
        }

        if (levelWon && !_levelWonSubscribed)
        {
            GlobalEvents.LevelWon.AddListener(LevelWonFunc);
            _levelWonSubscribed = true;
        }
        else if (!levelWon && _levelWonSubscribed)
        {
            GlobalEvents.LevelWon.RemoveListener(LevelWonFunc);
            _levelWonSubscribed = false;
        }

        if (levelLost && !_levelLostSubscribed)
        {
            GlobalEvents.LevelLost.AddListener(LevelLostFunc);
            _levelLostSubscribed = true;
        }
        else if (!levelLost && _levelLostSubscribed)
        {
            GlobalEvents.LevelLost.RemoveListener(LevelLostFunc);
            _levelLostSubscribed = false;
        }

        if (ballDestroyed && !_ballDestroyedSubscribed)
        {
            GlobalEvents.BallDestroyed.AddListener(BallDestroyedFunc);
            _ballDestroyedSubscribed = true;
        }
        else if (!ballDestroyed && _ballDestroyedSubscribed)
        {
            GlobalEvents.BallDestroyed.RemoveListener(BallDestroyedFunc);
            _ballDestroyedSubscribed = false;
        }

        if (ballSplit && !_ballSplitSubscribed)
        {
            GlobalEvents.BallSplitEvent.AddListener(BallSplitFunc);
            _ballSplitSubscribed = true;
        }
        else if (!ballSplit && _ballSplitSubscribed)
        {
            GlobalEvents.BallSplitEvent.RemoveListener(BallSplitFunc);
            _ballSplitSubscribed = false;
        }
    }

    private void LevelStartFunc()
    {
        Debug.Log("Level Start");
    }

    private void LevelWonFunc()
    {
        Debug.Log("levelWon");
    }

    private void LevelLostFunc()
    {
        Debug.Log("levelLost");
    }

    private void BallDestroyedFunc(PlayerBall playerBall)
    {
        Debug.Log($"ballDestroyed: playerBall={playerBall} ");
    }

    private void BallSplitFunc(PlayerBall ball, BallSplitter splitter)
    {
        Debug.Log($"ballSplitEvent: ball={ball} splitter={splitter}");
    }
#endif
}
