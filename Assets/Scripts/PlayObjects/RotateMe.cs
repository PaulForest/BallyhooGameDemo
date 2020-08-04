using UnityEngine;

namespace PlayObjects
{
    public class RotateMe : MonoBehaviour
    {
        private Transform _myTransform;
        [SerializeField] private float rotateRate = 0.1f;
        [SerializeField] private Vector3 rotationAxis = Vector3.forward;

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