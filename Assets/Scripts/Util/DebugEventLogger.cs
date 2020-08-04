using Balls;
using Level;
using PlayObjects;
using UnityEngine;

namespace Util
{
    public class DebugEventLogger : MonoBehaviour
    {
#if UNITY_EDITOR

        [Header("Which events should we log?")] [SerializeField]
        private bool levelStart;

        [SerializeField] private bool levelWon;
        [SerializeField] private bool levelLost;
        [SerializeField] private bool ballDestroyed;
        [SerializeField] private bool ballSplit;
        [SerializeField] private bool lastBallDestroyed;
        [SerializeField] private bool firstBallInGoal;
        [SerializeField] private bool lockedAreaUnlocked;

        private bool _levelStartSubscribed;
        private bool _levelWonSubscribed;
        private bool _levelLostSubscribed;
        private bool _ballDestroyedSubscribed;
        private bool _ballSplitSubscribed;
        private bool _lastBallDestroyedSubscribed;
        private bool _firstBallInGoalSubscribed;
        private bool _lockedAreaUnlockedSubscribed;

        private void OnValidate()
        {
            if (levelStart && !_levelStartSubscribed)
            {
                GlobalEvents.LevelStart.AddListener(OnLevelStartFunc);
                _levelStartSubscribed = true;
            }
            else if (!levelStart && _levelStartSubscribed)
            {
                GlobalEvents.LevelStart.RemoveListener(OnLevelStartFunc);
                _levelStartSubscribed = false;
            }

            if (levelWon && !_levelWonSubscribed)
            {
                GlobalEvents.LevelWon.AddListener(OnLevelWonFunc);
                _levelWonSubscribed = true;
            }
            else if (!levelWon && _levelWonSubscribed)
            {
                GlobalEvents.LevelWon.RemoveListener(OnLevelWonFunc);
                _levelWonSubscribed = false;
            }

            if (levelLost && !_levelLostSubscribed)
            {
                GlobalEvents.LevelLost.AddListener(OnLevelLostFunc);
                _levelLostSubscribed = true;
            }
            else if (!levelLost && _levelLostSubscribed)
            {
                GlobalEvents.LevelLost.RemoveListener(OnLevelLostFunc);
                _levelLostSubscribed = false;
            }

            if (ballDestroyed && !_ballDestroyedSubscribed)
            {
                GlobalEvents.BallDestroyed.AddListener(OnBallDestroyedFunc);
                _ballDestroyedSubscribed = true;
            }
            else if (!ballDestroyed && _ballDestroyedSubscribed)
            {
                GlobalEvents.BallDestroyed.RemoveListener(OnBallDestroyedFunc);
                _ballDestroyedSubscribed = false;
            }

            if (ballSplit && !_ballSplitSubscribed)
            {
                GlobalEvents.BallSplitEvent.AddListener(OnBallSplitFunc);
                _ballSplitSubscribed = true;
            }
            else if (!ballSplit && _ballSplitSubscribed)
            {
                GlobalEvents.BallSplitEvent.RemoveListener(OnBallSplitFunc);
                _ballSplitSubscribed = false;
            }

            if (lastBallDestroyed && !_lastBallDestroyedSubscribed)
            {
                GlobalEvents.LastBallDestroyed.AddListener(OnLastBallDestroyed);
                _lastBallDestroyedSubscribed = true;
            }
            else if (!lastBallDestroyed && _lastBallDestroyedSubscribed)
            {
                GlobalEvents.LastBallDestroyed.RemoveListener(OnLastBallDestroyed);
                _lastBallDestroyedSubscribed = false;
            }

            if (firstBallInGoal && !_firstBallInGoalSubscribed)
            {
                GlobalEvents.FirstBallInGoal.AddListener(OnFirstBallInGoal);
                _firstBallInGoalSubscribed = true;
            }
            else if (!firstBallInGoal && _firstBallInGoalSubscribed)
            {
                GlobalEvents.FirstBallInGoal.RemoveListener(OnFirstBallInGoal);
                _firstBallInGoalSubscribed = false;
            }

            if (lockedAreaUnlocked && !_lockedAreaUnlockedSubscribed)
            {
                GlobalEvents.LockedAreaUnlocked.AddListener(OnLockedAreaUnlocked);
                _lockedAreaUnlockedSubscribed = true;
            }
            else if (!lockedAreaUnlocked && _lockedAreaUnlockedSubscribed)
            {
                GlobalEvents.LockedAreaUnlocked.RemoveListener(OnLockedAreaUnlocked);
                _lockedAreaUnlockedSubscribed = false;
            }
        }

        private void OnLockedAreaUnlocked(LockedArea lockedArea)
        {
            Debug.Log($"{typeof(DebugEventLogger)}.OnLockedAreaUnlocked: lockedArea={lockedArea}");
        }

        private void OnFirstBallInGoal()
        {
            Debug.Log($"{typeof(DebugEventLogger)}.OnFirstBallInGoal()");
        }

        private void OnLastBallDestroyed()
        {
            Debug.Log($"{typeof(DebugEventLogger)}.OnLastBallDestroyed");
        }

        private void OnLevelStartFunc(LevelData levelData)
        {
            Debug.Log($"{typeof(DebugEventLogger)}.Level Start: levelData={levelData}");
        }

        private void OnLevelWonFunc(LevelData levelData)
        {
            Debug.Log($"{typeof(DebugEventLogger)}.levelWon: levelData={levelData}");
        }

        private void OnLevelLostFunc(LevelData levelData)
        {
            Debug.Log($"{typeof(DebugEventLogger)}.levelLost: levelData={levelData}");
        }

        private void OnBallDestroyedFunc(PlayerBall playerBall)
        {
            Debug.Log($"{typeof(DebugEventLogger)}.ballDestroyed: playerBall={playerBall} ");
        }

        private void OnBallSplitFunc(PlayerBall ball, BallSplitter splitter)
        {
            Debug.Log($"{typeof(DebugEventLogger)}.ballSplitEvent: ball={ball} splitter={splitter}");
        }
#endif
    }
}