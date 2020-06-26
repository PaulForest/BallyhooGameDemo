using Balls;
using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(CollideOnlyOncePlayerBall))]
    public class GoalArea : MonoBehaviour
    {
        public static int NumberOfBallsInGoal { get; private set; }

        public static bool HasBallsInGoal() => NumberOfBallsInGoal > 0;

        [SerializeField] private CollideOnlyOncePlayerBall collideOnlyOnce;

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

            if (NumberOfBallsInGoal == 1)
            {
                GlobalEvents.FirstBallInGoal?.Invoke();
            }
        }

        private void OnValidate()
        {
            if (!GetComponent<CollideOnlyOncePlayerBall>())
            {
                collideOnlyOnce = gameObject.AddComponent<CollideOnlyOncePlayerBall>();
            }
        }
    }
}
