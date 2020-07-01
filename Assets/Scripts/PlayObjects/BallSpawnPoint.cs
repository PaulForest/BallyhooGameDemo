using Balls;
using UnityEngine;

namespace PlayObjects
{
    public class BallSpawnPoint : MonoBehaviour
    {
        private void Start()
        {
            var ball = BallPool.Instance.GetAvailableObject();
            if (ball != null)
            {
                ball.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
        }
    }
}