using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.Mixer
{
    public class MixerApp : MonoBehaviour, IWindowedApp
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private float scaleOnOpen;
        private Vector2 initialPosition;
        private void Start()
        {
            gameObject.SetActive(false);
            initialPosition = gameObject.transform.position;
            closeButton.onClick.AddListener(CloseApp);
        }

        public void OpenApp()
        {
            OpenAppAsync().Forget();
        }
        private async UniTask OpenAppAsync()
        {
            gameObject.SetActive(true);
            closeButton.interactable = false;
            await gameObject.transform.DOScale(scaleOnOpen, 0.1f).SetLoops(2, LoopType.Yoyo);
            closeButton.interactable = true;
        }

        public void CloseApp()
        {
            gameObject.SetActive(false);
            gameObject.transform.position = initialPosition;
        }
    }
}
