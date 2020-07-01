namespace Balls
{
    public class PlayerBall : OnlyTouchOnce, IPoolableObject
    {
        private void OnEnable()
        {
            NumberOfNumberOfBallsInPlay++;
            ResetOnlyTouchOnceData();
        }

        private void OnDisable()
        {
            NumberOfNumberOfBallsInPlay--;
            GlobalEvents.BallDestroyed?.Invoke(this);

            if (!HasBallsInPlay)
            {
                GlobalEvents.LastBallDestroyed?.Invoke();
            }
        }

        private static bool HasBallsInPlay => NumberOfNumberOfBallsInPlay > 0;
        private static int NumberOfNumberOfBallsInPlay { get; set; }
    }
}