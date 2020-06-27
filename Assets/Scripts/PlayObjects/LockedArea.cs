using Balls;
using TMPro;
using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(TMP_Text))]
    public class LockedArea : MonoBehaviour
    {
        [SerializeField] private int collisionCount;
        [SerializeField] private TMP_Text splitCountLabel;

        private void Start()
        {
            splitCountLabel.text = collisionCount.ToString();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            var ball = other.collider.GetComponent<PlayerBall>();
            if (!ball) return;

            Destroy(ball.gameObject);
            collisionCount--;
            
            splitCountLabel.text = collisionCount.ToString();

            if (collisionCount > 0) return;

            GlobalEvents.LockedAreaUnlocked?.Invoke(this);

            Destroy(gameObject);
        }
    }
}
