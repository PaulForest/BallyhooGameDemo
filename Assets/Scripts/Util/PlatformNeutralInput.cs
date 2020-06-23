using UnityEngine;

namespace Util
{
    public static class PlatformNeutralInput
    {
#if UNITY_EDITOR
        private static Vector3 _oldMousePos = new Vector3();
#endif

        public static void GetDeltaXDeltaY(ref float dx, ref float dy)
        {
#if UNITY_EDITOR
            dx = Input.mousePosition.x - _oldMousePos.x;
            dy = Input.mousePosition.y - _oldMousePos.y;
            _oldMousePos = Input.mousePosition;
#else
            if (Input.touchCount <= 0) return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Moved) return;

            dx = touch.deltaPosition.x;
            dy = touch.deltaPosition.y;
#endif
        }
    }
}
