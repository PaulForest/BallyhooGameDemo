﻿using UnityEngine;

namespace Balls
{
    public class OnlyTouchOnce : MonoBehaviour
    {
        /// <summary>
        /// This is a bitfield of all the instances of <see cref="CollideOnlyOnce{TOnlyTouchOnce,TUnityEvent}"/> this
        /// instance has interacted with.
        /// </summary>
        [HideInInspector] public int mInteractedWithBitField;

        private void Awake()
        {
            mInteractedWithBitField = 0;
        }
    }
}