using UnityEngine;

namespace UI
{
    /// <summary>
    /// Rotates the camera slightly to the left or right, and changes the gravity to turn slightly to the left or right
    /// </summary>
    public class SwipeGameplayController : MonoBehaviour
    {
        [Header("Changes to the camera in response to the slider's value")] [SerializeField]
        private Vector3 minCameraAngle;

        [SerializeField] private Vector3 maxCameraAngle;
        [SerializeField] private Camera shiftCamera;

        [Header("Change to gravity in response to the slider's value")] [SerializeField]
        private Vector3 minGravityAngle;

        [SerializeField] private Vector3 maxGravityAngle;

        [HideInInspector] private float _currentValue = 0.5f;
        [HideInInspector] private int _screenWidth;

        private void Start()
        {
            if (!shiftCamera) shiftCamera = Camera.main;
            if (!shiftCamera) shiftCamera = Camera.current;
            if (!shiftCamera) Debug.LogError("I need my camera set please");

            _screenWidth = Screen.width / 2;
        }

        public void Update()
        {
            if (Input.touchCount <= 0) return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Moved) return;

            _currentValue += touch.deltaPosition.x / _screenWidth;
            _currentValue = Mathf.Clamp01(_currentValue);

            var rot = Vector3.Lerp(minCameraAngle, maxCameraAngle, _currentValue);
            shiftCamera.transform.rotation = Quaternion.Euler(rot);

            Physics.gravity = Vector3.Lerp(minGravityAngle, maxGravityAngle, _currentValue);
        }
    }
}
