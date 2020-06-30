using UnityEngine;

namespace PlayObjects
{
    public class RotateMe : MonoBehaviour
    {
        [SerializeField] private Vector3 rotationAxis = Vector3.forward;
        [SerializeField] private float rotateRate = 0.1f;

        private Transform _myTransform;

        public void Start()
        {
            _myTransform = GetComponent<Transform>();
        }

        private void Update()
        {
            _myTransform.Rotate(rotationAxis * (rotateRate * Time.deltaTime));
        }
    }
}
