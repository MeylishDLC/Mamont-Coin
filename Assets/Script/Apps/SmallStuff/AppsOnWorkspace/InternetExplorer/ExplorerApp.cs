using UnityEngine;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.InternetExplorer
{
    public class ExplorerApp: BasicWorkspaceApp
    {
        [SerializeField] private Tab[] tabs;
        protected override void Start()
        {
            base.Start();
            tabs[0].OpenTab();
        }
    }
}