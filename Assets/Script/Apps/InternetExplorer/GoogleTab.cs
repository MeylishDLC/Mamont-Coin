using TMPro;
using UnityEngine;

namespace Script.Apps.InternetExplorer
{
    public class GoogleTab: Tab
    {
        [SerializeField] private TMP_InputField searchField;
        [field: SerializeField] private Tab[] searchableTabs;
        protected override void Awake()
        {
            base.Awake();
            searchField.onEndEdit.AddListener(OnSearchEntered);
        }
        private void OnSearchEntered(string searchText)
        {
            foreach (var tab in searchableTabs)
            {
                CheckSearch(tab, searchText);
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