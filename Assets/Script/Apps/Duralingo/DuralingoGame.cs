using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.Duralingo
{
    public class DuralingoGame: MonoBehaviour
    {
        [SerializeField] private TextField textField;
        [SerializeField] private Button SubmitButton;

        private void Start()
        {
            SubmitButton.onClick.AddListener(OnButtonSubmit);
        }

        private void OnButtonSubmit()
        {
            SubmitButton.interactable = false;
            
            if (textField.CheckAccuracy())
            {
                Debug.Log("You won!");
            }
            else
            {
                Debug.Log("You lose");
            }
            //todo: some message here about passing or failing on event???
        }
    }
}