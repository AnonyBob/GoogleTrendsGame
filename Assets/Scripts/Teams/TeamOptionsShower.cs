using UnityEngine;
using UnityEngine.EventSystems;

namespace GoogleTrends.Teams
{
    public class TeamOptionsShower : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private RectTransform _options;

        [SerializeField]
        private Vector2 _showPosition;

        [SerializeField]
        private Vector2 _hidePosition;

        private bool _showing;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_showing)
            {
                HideOptions();
            }
            else
            {
                ShowOptions();    
            }
            
        }

        private void ShowOptions()
        {
            _options.anchoredPosition = _showPosition;
            _showing = true;
        }

        private void HideOptions()
        {
            _options.anchoredPosition = _hidePosition;
            _showing = false;
        }
    }
}