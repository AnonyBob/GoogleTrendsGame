using UnityEngine;

namespace GoogleTrends.UI.Popups
{
    public class PopupOpener : MonoBehaviour
    {
        [SerializeField]
        private GameObject _popupPrefab;

        public void OpenPopup()
        {
            PopupManager.OpenPopup(_popupPrefab);
        }
    }
}