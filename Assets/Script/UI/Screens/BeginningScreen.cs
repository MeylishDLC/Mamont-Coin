using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginningScreen : MonoBehaviour
{
    public void OnPasswordEnter()
    {
        //todo: maybe some funny check for password
        SceneManager.LoadScene(1);
    }
}
