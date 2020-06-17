using UnityEngine.Events;

public static class GlobalEvents
{
    public static readonly UnityEvent LevelStart = new UnityEvent();
    public static readonly UnityEvent LevelWon = new UnityEvent();
    public static readonly UnityEvent LevelLost = new UnityEvent();
    public static readonly UnityPlayerBallEvent BallDestroyed = new UnityPlayerBallEvent();
    public static readonly UnityPlayerBallPlayerSplitterEvent BallSplitEvent = new UnityPlayerBallPlayerSplitterEvent();
}

public class UnityStringEvent : UnityEvent<string>
{
}

public class UnityPlayerBallEvent : UnityEvent<PlayerBall>
{
}

public class UnityPlayerBallPlayerSplitterEvent : UnityEvent<PlayerBall, BallSplitter>
{
}

