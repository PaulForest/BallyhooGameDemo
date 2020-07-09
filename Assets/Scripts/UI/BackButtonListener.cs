using UnityEngine;

namespace UI
{
    public class BackButtonListener : MonoBehaviour
    {
        public static BackButtonListener Instance
        {
            get
            {
                if (_instance) return _instance;
                var go = new GameObject("BackButtonListener");
                _instance = go.AddComponent<BackButtonListener>();

                return _instance;
            }
        }

        private static BackButtonListener _instance;

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(this);
                return;
            }

            _instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

#if UNITY_ANDROID
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GlobalEvents.BackButtonPressed?.Invoke();
            }
        }
#endif
    }
}