using UnityEngine;

public class LockedArea : MonoBehaviour
{
    [SerializeField] private int collisionCount;

    private void OnCollisionEnter(Collision other)
    {
        var ball = other.collider.GetComponent<PlayerBall>();
        if (!ball) return;

        Destroy(ball.gameObject);
        collisionCount--;

        if (collisionCount > 0) return;

        GlobalEvents.LockedAreaUnlocked?.Invoke(this);

        Destroy(gameObject);
    }
}
