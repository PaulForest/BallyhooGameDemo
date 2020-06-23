using System;
using UnityEngine;

namespace Util
{
    public class CameraPreference : MonoBehaviour
    {
        public enum CameraSelectionBehaviour
        {
            NoPreference,
            KeepMeDeleteOther,
            DeleteMeKeepOther
        }

        [Header("In-editor only, choose which option you prefer to use")]
        public CameraSelectionBehaviour inEditorSelectionBehaviour;

        [Header("In-game only, choose which option you prefer to use")]
        public CameraSelectionBehaviour inGameSelectionBehaviour;

        public CameraSelectionBehaviour ChosenBehaviour =>
#if UNITY_EDITOR
            inEditorSelectionBehaviour;
#else
            inGameSelectionBehaviour;
#endif

        private Camera MyCamera { get; set; }

        public static Camera GetPreferredOption =>
            _preferredChoice && _preferredChoice.MyCamera ? _preferredChoice.MyCamera : null;

        public static CameraPreference PreferredChoice => _preferredChoice;
        private static CameraPreference _preferredChoice;

        private void Awake()
        {
            MyCamera = GetComponent<Camera>();
            if (!MyCamera) MyCamera = GetComponentInChildren<Camera>();
            if (!MyCamera) MyCamera = GetComponentInParent<Camera>();

            if (MyCamera) return;

            Debug.LogError(
                $"{this} should be placed on a camera that you don't want to use in production, but might for quick debugging");
        }

        private void Start()
        {
            var oldChoice = _preferredChoice;
            var newChoice = MakeChoice();
            _preferredChoice = newChoice;

            if (newChoice != oldChoice)
            {
                GlobalEvents.CameraChanged?.Invoke(newChoice.MyCamera);
            }
        }

        protected CameraPreference MakeChoice()
        {
            CameraPreference newChoice = null;

            switch (ChosenBehaviour)
            {
                case CameraSelectionBehaviour.KeepMeDeleteOther:
                    newChoice = this;
                    break;
                case CameraSelectionBehaviour.NoPreference:
                    return _preferredChoice;
            }

            foreach (var c in FindObjectsOfType<CameraPreference>())
            {
                if (this == c) continue;

                switch (ChosenBehaviour)
                {
                    case CameraSelectionBehaviour.KeepMeDeleteOther:
                        DestroyImmediate(c.gameObject);
                        break;
                    case CameraSelectionBehaviour.DeleteMeKeepOther:
                        DestroyImmediate(gameObject);
                        newChoice = c;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (newChoice == null)
            {
                Debug.LogError($"{this} Something is wrong, couldn't set the preferred choice");
                return _preferredChoice;
            }

            _preferredChoice = newChoice;

            return newChoice;
        }
    }
}
