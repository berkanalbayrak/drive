using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectDrive.UI
{
    public class SceneReloader : MonoBehaviour
    {
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}