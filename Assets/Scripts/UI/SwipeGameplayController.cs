using UnityEngine;
using Util;

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

        [HideInInspector] private float _dx;

        private void Awake()
        {
            GlobalEvents.CameraChanged.AddListener(OnCameraChanged);
        }

        private void OnCameraChanged(Camera arg0)
        {
            shiftCamera = arg0;
        }

        private void Start()
        {
            if (!shiftCamera) shiftCamera = Camera.main;
            if (!shiftCamera) shiftCamera = Camera.current;
            if (!shiftCamera) shiftCamera = CameraPreference.GetPreferredOption;
            if (!shiftCamera) shiftCamera = FindObjectOfType<Camera>();
            if (!shiftCamera) Debug.LogError("I need my camera set please");

            _screenWidth = Screen.width / 2;
        }

        public void Update()
        {
            if (!shiftCamera) return;

            _dx = 0.5f;

            PlatformNeutralInput.GetDeltaXDeltaY(ref _dx);

            _currentValue += _dx / _screenWidth;
            _currentValue = Mathf.Clamp01(_currentValue);

            var rot = Vector3.Lerp(minCameraAngle, maxCameraAngle, _currentValue);
            shiftCamera.transform.rotation = Quaternion.Euler(rot);

            Physics.gravity = Vector3.Lerp(minGravityAngle, maxGravityAngle, _currentValue);
        }

        private void OnDestroy()
        {
            GlobalEvents.CameraChanged.RemoveListener(OnCameraChanged);
        }
    }
}
