using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Screens
{
    public class BeginningScreen : MonoBehaviour
    {
        public void OnPasswordEnter()
        {
            //todo: maybe some funny check for password
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
      
    }
}
