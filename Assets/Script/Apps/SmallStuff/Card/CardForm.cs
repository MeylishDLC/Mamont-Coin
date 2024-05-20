using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Apps.SmallStuff.Card
{
    public class CardForm : MonoBehaviour
    {
        //todo: check set input and restrict all wrong variations
        public bool numberSet { get; set; }
        public bool nameSet { get; set; }
        public bool daySet { get; set; }
        public bool yearSet { get; set; }
        public bool CVVSet { get; set; }
    
        private void Update()
        {
            if (numberSet && nameSet && daySet && yearSet && CVVSet)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
