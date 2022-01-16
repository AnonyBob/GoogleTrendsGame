using UnityEngine;
using UnityEngine.Events;

namespace GoogleTrends.UI.Popups
{
    public class PopupCloser : MonoBehaviour
    {
        [SerializeField]
        private GameObject _popupRoot;

        [SerializeField]
        private UnityEvent _onPopupClose;

        public void ClosePopup()
        {
            if (_popupRoot == null)
            {
                _popupRoot = gameObject;
            }
            
            PopupManager.ClosePopup(_popupRoot);
            _onPopupClose.Invoke();
        }
    }
}