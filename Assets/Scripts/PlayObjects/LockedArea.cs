using Balls;
using TMPro;
using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(TMP_Text), typeof(Animation))]
    public class LockedArea : MonoBehaviour
    {
        [SerializeField] private int collisionCount;
        [SerializeField] private TMP_Text splitCountLabel;

        [Header("When this is unlocked, we create this many objects.")] 
        [SerializeField] private int explosionCount;
        [SerializeField] private bool useCollisionCountForExplosionCount = true;
        

        private Animation _animation;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
            if (!_animation)
            {
                Debug.LogError($"{this}: I need an animation", this);
            }

            if (useCollisionCountForExplosionCount)
            {
                explosionCount = collisionCount;
            }
        }

        private void Start()
        {
            splitCountLabel.text = collisionCount.ToString();
        }

        private void OnCollisionEnter(Collision other)
        {
            var ball = other.collider.GetComponent<PlayerBall>();
            if (!ball) return;

            BallPool.Instance.ReturnObject(ball.gameObject);
            collisionCount--;

            splitCountLabel.text = collisionCount.ToString();

            if (collisionCount > 0)
            {
                if (!_animation) return;
                
                _animation.Stop();
                _animation.Play();

                return;
            }

            GlobalEvents.LockedAreaUnlocked?.Invoke(this);
            var go = new GameObject("Explosion");
            BallSpawnPoint.AddNewInstance(go, go.transform.position, new CollideOnlyOnceData
                {
                    MyBitFieldMask = -1
                }, 100,
                0.1f);
            // Destroy(go, 2f);
            

            Destroy(gameObject);
        }
    }
}