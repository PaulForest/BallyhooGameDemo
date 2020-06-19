using UnityEngine;

[RequireComponent(typeof(CollideOnlyOnce))]
public class GoalArea : MonoBehaviour
{
    public static int NumberOfBallsInGoal { get; private set; }
    public static bool HasBallsInGoal() => NumberOfBallsInGoal > 0;

    private CollideOnlyOnce _collideOnlyOnce;

    private void Start()
    {
        NumberOfBallsInGoal = 0;
        _collideOnlyOnce = GetComponent<CollideOnlyOnce>();
        _collideOnlyOnce.onCollisionEvent.AddListener(OnCollideWithBall);
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
}
