using UnityEngine;

namespace PlayObjects
{
    public class KillZone : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            Destroy(other.gameObject);
        }
    }
}
