﻿using UnityEngine;

namespace Util
{
    public static class PlatformNeutralInput
    {
#if UNITY_EDITOR
        private static Vector3 _oldMousePos;
#endif

        public static void Reset()
        {
#if UNITY_EDITOR
            _oldMousePos = Input.mousePosition;
#endif
        }

        public static void GetDeltaXDeltaY(ref float dx)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                dx = Input.mousePosition.x - _oldMousePos.x;
            }

            _oldMousePos = Input.mousePosition;
#else
            if (Input.touchCount <= 0) return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Moved) return;

            dx = touch.deltaPosition.x;
#endif
        }
    }
}