using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Rotates the camera slightly to the left or right, and changes the gravity to turn slightly to the left or right
    /// </summary>
    public class GameplaySlider : MonoBehaviour
    {
        [Header("Changes to the camera in response to the slider's value")]
        [SerializeField] private Vector3 minCameraAngle;
        [SerializeField] private Vector3 maxCameraAngle;
        [SerializeField] private Camera shiftCamera;

        [Header("Change to gravity in response to the slider's value")]
        [SerializeField] private Vector3 minGravityAngle;
        [SerializeField] private Vector3 maxGravityAngle;

        [SerializeField] private Slider slider;

        public void OnSliderChange()
        {
            var rot = Vector3.Lerp(minCameraAngle, maxCameraAngle, slider.value);
            shiftCamera.transform.rotation = Quaternion.Euler(rot);

            Physics.gravity = Vector3.Lerp(minGravityAngle, maxGravityAngle, slider.value);
        }
    }
}
