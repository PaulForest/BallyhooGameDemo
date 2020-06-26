using System;

namespace Balls
{
    public class PlayerBall : OnlyTouchOnce
    {
        private void Awake()
        {
            NumberOfNumberOfBallsInPlay++;
        }

        private void OnDestroy()
        {
            NumberOfNumberOfBallsInPlay--;
            GlobalEvents.BallDestroyed?.Invoke(this);

            if (!HasBallsInPlay)
            {
                GlobalEvents.LastBallDestroyed?.Invoke();
            }
        }

        public static bool HasBallsInPlay => NumberOfNumberOfBallsInPlay > 0;
        public static int NumberOfNumberOfBallsInPlay { get; private set; }
    }
}
