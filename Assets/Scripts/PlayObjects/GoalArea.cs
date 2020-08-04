using Balls;
using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(CollideOnlyOncePlayerBall))]
    public class GoalArea : MonoBehaviour
    {
        [SerializeField] private CollideOnlyOncePlayerBall collideOnlyOnce;
        public static int NumberOfBallsInGoal { get; private set; }

        public static bool HasBallsInGoal()
        {
            return NumberOfBallsInGoal > 0;
        }

        private void Start()
        {
            NumberOfBallsInGoal = 0;
            collideOnlyOnce = GetComponent<CollideOnlyOncePlayerBall>();
            collideOnlyOnce.onCollisionEvent.AddListener(OnCollideWithBall);
        }

        private void OnCollideWithBall(PlayerBall ball)
        {
            if (!ball) return;

            NumberOfBallsInGoal++;

            if (NumberOfBallsInGoal == 1) GlobalEvents.FirstBallInGoal?.Invoke();

            BallPool.Instance.ReturnObject(ball.gameObject);
        }

        private void OnValidate()
        {
            if (!GetComponent<CollideOnlyOncePlayerBall>())
                collideOnlyOnce = gameObject.AddComponent<CollideOnlyOncePlayerBall>();
        }
    }
}