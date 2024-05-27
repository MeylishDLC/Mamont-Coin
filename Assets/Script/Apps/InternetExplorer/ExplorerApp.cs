using Script.Apps.SmallStuff.AppsOnWorkspace;
using UnityEngine;

namespace Script.Apps.InternetExplorer
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