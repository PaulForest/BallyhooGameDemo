using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class Cheats : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
