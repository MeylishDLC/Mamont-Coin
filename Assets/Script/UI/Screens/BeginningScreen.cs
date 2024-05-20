using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.UI.Screens
{
    public class BeginningScreen : MonoBehaviour
    {
        [FMODUnity.BankRef]
        public List<string> banks;
        public void OnPasswordEnter()
        {
            //todo: maybe some funny check for password
            LoadBanks();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        private void LoadBanks()
        {
            foreach (string b in banks)
            {
                FMODUnity.RuntimeManager.LoadBank(b, true);
                Debug.Log("Loaded bank " + b);
            }
            /*
                For Chrome / Safari browsers / WebGL.  Reset audio on response to user interaction (LoadBanks is called from a button press), to allow audio to be heard.
            */
            FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
            FMODUnity.RuntimeManager.CoreSystem.mixerResume();
            
            StartCoroutine(CheckBanksLoaded());
        }

        IEnumerator CheckBanksLoaded()
        {
            while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
            {
                yield return null;
            }
        }
    }
}
