using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Rotates the camera slightly to the left or right, and changes the gravity to turn slightly to the left or right
    /// </summary>
    public class GameplaySlider : MonoBehaviour
    {
        [Header("Changes to the camera in response to the slider's value")] [SerializeField]
        private Vector3 minCameraAngle;

        [SerializeField] private Vector3 maxCameraAngle;
        [SerializeField] private Camera shiftCamera;

        [Header("Change to gravity in response to the slider's value")] [SerializeField]
        private Vector3 minGravityAngle;

        [SerializeField] private Vector3 maxGravityAngle;

        [SerializeField] private Slider slider;

        private void Start()
        {
            if (!shiftCamera) shiftCamera = Camera.current;
            if (!shiftCamera) Debug.LogError("I need my camera set please");

            if (!slider) slider = GetComponent<Slider>();
            if (!slider) slider = GetComponentInParent<Slider>();
            if (!slider) slider = GetComponentInChildren<Slider>();
            if (!slider) Debug.LogError("I need my slider set please");
        }

        public void OnSliderChange()
        {
            var rot = Vector3.Lerp(minCameraAngle, maxCameraAngle, slider.value);
            shiftCamera.transform.rotation = Quaternion.Euler(rot);

            Physics.gravity = Vector3.Lerp(minGravityAngle, maxGravityAngle, slider.value);
        }
    }
}
