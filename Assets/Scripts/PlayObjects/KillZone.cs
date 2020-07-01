using Balls;
using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class KillZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            OnTriggerEnter(other.collider);
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerBall = other.GetComponent<PlayerBall>();
            if (playerBall)
            {
                BallPool.Instance.ReturnObject(playerBall.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }

        private void OnValidate()
        {
            if (GetComponent<Rigidbody>()) return;

            var a = gameObject.AddComponent<Rigidbody>();
            a.isKinematic = true;
            a.useGravity = false;

            var b = GetComponent<Collider>();
            if (!b)
            {
                Debug.LogError("I need a collider of some kind", this);
            }
            else
            {
                b.isTrigger = true;
            }
        }
    }
}