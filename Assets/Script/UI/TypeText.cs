using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Script.UI
{
    public class TypeText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private int typingSpeedMilliseconds;

        private string fullText;
        private int currentCharacterIndex;

        private void Start()
        {
            fullText = textMeshProUGUI.text;
            textMeshProUGUI.text = "";
            StartTypeText().Forget();
        }

        private async UniTask StartTypeText()
        {
            while (currentCharacterIndex < fullText.Length)
            {
                textMeshProUGUI.text += fullText[currentCharacterIndex];
                currentCharacterIndex++;
                await UniTask.Delay(typingSpeedMilliseconds);
            }
        }
    }
}
