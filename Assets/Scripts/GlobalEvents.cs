using UnityEngine.Events;

public static class GlobalEvents
{
    public static readonly UnityEvent LevelStart = new UnityEvent();
    public static readonly UnityEvent LevelWon = new UnityEvent();
    public static readonly UnityEvent LevelLost = new UnityEvent();
    public static readonly UnityPlayerBallEvent BallDestroyed = new UnityPlayerBallEvent();
    public static readonly UnityEvent LastBallDestroyed = new UnityEvent();
    public static readonly UnityPlayerBallPlayerSplitterEvent BallSplitEvent = new UnityPlayerBallPlayerSplitterEvent();
    public static readonly UnityEvent FirstBallInGoal = new UnityEvent();
}

public class UnityPlayerBallEvent : UnityEvent<PlayerBall>
{
}

public class UnityPlayerBallPlayerSplitterEvent : UnityEvent<PlayerBall, BallSplitter>
{
}
