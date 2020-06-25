using Balls;
using Level;
using PlayObjects;
using UnityEngine;
using UnityEngine.Events;

public static class GlobalEvents
{
    public static readonly UnityEvent MainMenuShown = new UnityEvent();
    public static readonly UnityLevelDataEvent LevelStart = new UnityLevelDataEvent();
    public static readonly UnityLevelDataEvent LevelWon = new UnityLevelDataEvent();
    public static readonly UnityLevelDataEvent LevelLost = new UnityLevelDataEvent();
    public static readonly UnityPlayerBallEvent BallDestroyed = new UnityPlayerBallEvent();
    public static readonly UnityEvent LastBallDestroyed = new UnityEvent();
    public static readonly UnityPlayerBallPlayerSplitterEvent BallSplitEvent = new UnityPlayerBallPlayerSplitterEvent();
    public static readonly UnityEvent FirstBallInGoal = new UnityEvent();
    public static readonly UnityLockedAreaEvent LockedAreaUnlocked = new UnityLockedAreaEvent();

    public static readonly UnityCameraEvent CameraChanged = new UnityCameraEvent();
}

public class UnityLevelDataEvent : UnityEvent<LevelData>
{

}

public class UnityPlayerBallEvent : UnityEvent<PlayerBall>
{
}

public class UnityPlayerBallPlayerSplitterEvent : UnityEvent<PlayerBall, BallSplitter>
{
}

public class UnityLockedAreaEvent : UnityEvent<LockedArea>
{
}

public class UnityCameraEvent : UnityEvent<Camera>
{
}
