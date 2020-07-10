using UnityEngine;

namespace Util
{
    public class StopTimeWhileAlive : MonoBehaviour
    {
        private float _oldTimeScale;

        private void Awake()
        {
            _oldTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            Time.timeScale = _oldTimeScale;
        }
    }
}