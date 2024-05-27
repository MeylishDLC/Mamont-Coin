using TMPro;
using UnityEngine;

namespace Script.Apps.ChatScript.Skamp
{
    [RequireComponent(typeof(RectTransform))]
    public class BackgroundResizer : MonoBehaviour
    {
        [SerializeField] private GameObject image;
        [SerializeField] private GameObject text;
        [SerializeField] private float paddingX;
        [SerializeField] private float paddingY;
        [SerializeField] private float minHeight; 

        private RectTransform imageRectTransform;
        private TextMeshProUGUI textMeshPro;

        private void Start()
        {
            imageRectTransform = image.GetComponent<RectTransform>();
            textMeshPro = text.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            textMeshPro.ForceMeshUpdate();
            var preferredHeight = textMeshPro.GetPreferredValues().y;
            var newHeight = Mathf.Max(preferredHeight + paddingY, minHeight);
            imageRectTransform.sizeDelta = new Vector2(imageRectTransform.sizeDelta.x + paddingX, newHeight);
        }
    }
}
