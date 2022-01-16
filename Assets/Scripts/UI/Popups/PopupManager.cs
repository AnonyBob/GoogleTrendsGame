using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoogleTrends.UI.Popups
{
    public class PopupManager : MonoSingleton<PopupManager>
    {
        [SerializeField]
        private Transform _popupRoot;
        
        private List<PopupData> _popupStack = new List<PopupData>();

        public static void OpenPopup(GameObject popupPrefab, bool allowMoreThanOne = false)
        {
            if (!allowMoreThanOne)
            {
                if (Instance._popupStack.Any(pd => pd.Prefab == popupPrefab))
                {
                    return;
                }
            }

            var instance = Instantiate(popupPrefab, Instance._popupRoot);
            Instance._popupStack.Add(new PopupData()
            {
                Instance = instance,
                Prefab = popupPrefab
            });
        }
        
        public static void ClosePopup(GameObject instance)
        {
            var index = Instance._popupStack.FindIndex(pd => pd.Instance == instance);
            if (index >= 0 && index < Instance._popupStack.Count)
            {
                Instance._popupStack.RemoveAt(index);
            }
        }
    }
}