using GoogleTrends.GameManagers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoogleTrends.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour, IPointerClickHandler
    {
        private Button _button;

        [SerializeField]
        private AudioClip _enabledSound;

        [SerializeField]
        private AudioClip _disabledSound;

        private void Start()
        {
            _button = GetComponent<Button>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager.PlaySound(_button.interactable ? _enabledSound : _disabledSound);
        }
    }
}