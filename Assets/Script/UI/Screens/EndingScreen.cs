using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Script.UI.Screens
{
    public class EndingScreen : MonoBehaviour
    {
        //todo: ????? some ending better than this shit
        private TypeText text;
        private void Start()
        {
            text = gameObject.GetComponentInChildren<TypeText>();
            text.enabled = false;
            text.gameObject.SetActive(false);
        
            OnAppearAsync().Forget();
        }

        private async UniTask OnAppearAsync()
        {
            await gameObject.transform.DOScale(1, 0.1f).ToUniTask();
            text.gameObject.SetActive(true);
            text.enabled = true;
        } 
    }
}
