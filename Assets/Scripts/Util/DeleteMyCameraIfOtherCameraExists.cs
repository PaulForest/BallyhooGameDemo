using UnityEngine;

namespace Util
{
    public class DeleteMyCameraIfOtherCameraExists : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            #if UNITY_EDITOR

            Camera myCamera = GetComponent<Camera>();
            if (!myCamera)
            {
                Debug.LogError($"{this} should be placed on a camera that you don't want to use in production, but might for quick debugging");
                return;
            }

            foreach (Camera c in FindObjectsOfType<Camera>())
            {
                if (myCamera == c)
                {
                    DestroyImmediate(gameObject);
                    return;
                }
            }
            #endif

        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
