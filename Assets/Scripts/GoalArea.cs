using System;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    public static int NumberOfBallsInGoal { get; private set; }

    public static bool HasBallsInGoal() => NumberOfBallsInGoal > 0;

    private void Start()
    {
        NumberOfBallsInGoal = 0;
    }

    private void OnCollisionEnter(Collision other)
    {
        var ball = other.collider.GetComponent<PlayerBall>();
        if (!ball) return;

        NumberOfBallsInGoal++;
        Destroy(ball.gameObject);

        if (NumberOfBallsInGoal == 1)
        {
            GlobalEvents.FirstBallInGoal?.Invoke();
        }
    }
}
