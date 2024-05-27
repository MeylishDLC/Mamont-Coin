using Cysharp.Threading.Tasks;
using Script.Core.Popups;
using Script.Core.Popups.Spawns;
using UnityEngine;

namespace Script.Apps.SmallStuff.AppsOnWorkspace
{
    public class AmigoApp: BasicWorkspaceApp
    {
        [SerializeField] private GameObject lagScreen;
        [SerializeField] private int delayBeforeLagMilliseconds;
        [SerializeField] private Popup[] adPopupsPrefabs;
        [SerializeField] private int spawnAmount;
        [SerializeField] private RandomSpawner randomSpawner;

        private PopupContainer popupContainer;
        public override void OpenApp()
        {
            popupContainer = FindAnyObjectByType<PopupContainer>();
            
            base.OpenApp();
            SpawnPopups().Forget();
        }

        private async UniTask SpawnPopups()
        {
            await UniTask.Delay(delayBeforeLagMilliseconds);
            
            lagScreen.SetActive(true);

            for (int i = 0; i < spawnAmount; i++)
            {
                foreach (var adPopup in adPopupsPrefabs)
                {
                    var pos = randomSpawner.GetRandomPosition(popupContainer);

                    var popup = Instantiate(adPopup, popupContainer.transform);
                    popup.OpenApp();
                    popup.transform.localPosition = pos;
                }
            }
        }
        
    }
}