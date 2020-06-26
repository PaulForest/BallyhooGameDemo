using UnityEngine;

namespace PlayObjects
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class KillZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Destroy(other.gameObject);
        }

        private void OnValidate()
        {
            if (GetComponent<Rigidbody>()) return;

            var a = gameObject.AddComponent<Rigidbody>();
            a.isKinematic = true;
            a.useGravity = false;
        }
    }
}
