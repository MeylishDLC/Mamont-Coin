using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.InternetExplorer
{
    public class Tab: MonoBehaviour
    {
        [field:SerializeField] protected Button OpenButton { get; private set; }
        [field:SerializeField] protected Sprite ActiveTabSprite { get; private set; }
        [field:SerializeField] protected Sprite InactiveTabSprite { get; private set; }

        private bool isOpen;
        private Image tabImage;
        
        public static event Action<Tab> OnTabOpen; 
        protected virtual void Awake()
        {
            tabImage = OpenButton.GetComponent<Image>();
            tabImage.sprite = InactiveTabSprite;
            gameObject.SetActive(false);

            OpenButton.onClick.AddListener(OpenTab);
            OnTabOpen += CloseTab;
        }
        
        public virtual void OpenTab()
        {
            if (isOpen)
            {
                return;
            }
            
            tabImage.sprite = ActiveTabSprite;
            gameObject.SetActive(true);
            isOpen = true;
            OnTabOpen?.Invoke(this);
        }

        public virtual void CloseTab(Tab tab)
        {
            if (tab == this)
            {
                return;
            }
            gameObject.SetActive(false);
            tabImage.sprite = InactiveTabSprite;
            isOpen = false;
        }
    }
}