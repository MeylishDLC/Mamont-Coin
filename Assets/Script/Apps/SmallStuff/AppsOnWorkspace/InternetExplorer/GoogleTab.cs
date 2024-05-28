using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.InternetExplorer
{
    public class GoogleTab: Tab
    {
        [SerializeField] private TMP_InputField searchField;
        [field: SerializeField] private Tab[] searchableTabs;

        [Header("Parent Control")] 
        [SerializeField] private GameObject parentControlScreen;
        [SerializeField] private Button parentControlButton;
        
        private bool found;
        protected override void Awake()
        {
            base.Awake();
            searchField.onEndEdit.AddListener(OnSearchEntered);
            parentControlButton.onClick.AddListener(ReturnToGoogle);
        }
        private void ReturnToGoogle()
        {
            parentControlScreen.SetActive(false);
        }
        private void OnSearchEntered(string searchText)
        {
            found = false;
            foreach (var tab in searchableTabs)
            {
                CheckSearch(tab, searchText);
            }

            if (!found && searchText != "")
            {
                parentControlScreen.SetActive(true);
            }
        }

        private void CheckSearch(Tab tab, string search)
        {
            if (tab is ISearchable searchableTab)
            {
                foreach (var variation in searchableTab.SearchVariations)
                {
                    if (variation == search)
                    { 
                        searchableTab.ActionOnSearch();
                        found = true;
                    }
                }
            }
            else
            {
                Debug.LogError($"Tab type of {tab.GetType()} is not a searchable type");
            }
        }
    }
}