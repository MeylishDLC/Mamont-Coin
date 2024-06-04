using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Screens
{
    public class BeginningScreen : MonoBehaviour
    {
        public void OnPasswordEnter()
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
      
    }
}
