﻿using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    ///     Rotates the camera slightly to the left or right, and changes the gravity to turn slightly to the left or right
    /// </summary>
    public class SwipeGameplayController : MonoBehaviour
    {
        [HideInInspector] private float _currentValue = 0.5f;

        [HideInInspector] private float _dx;
        [HideInInspector] private int _screenWidth;

        [SerializeField] private Vector3 maxCameraAngle;

        [SerializeField] private Vector3 maxGravityAngle;

        [Header("Changes to the camera in response to the slider's value")] [SerializeField]
        private Vector3 minCameraAngle;

        [Header("Change to gravity in response to the slider's value")] [SerializeField]
        private Vector3 minGravityAngle;

        [SerializeField] private Camera shiftCamera;

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

            PlatformNeutralInput.Reset();
        }

        public void Update()
        {
            if (!shiftCamera) return;

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