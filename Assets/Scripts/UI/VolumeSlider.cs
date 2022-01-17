using System;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.UI
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.value = AudioListener.volume;
            _slider.onValueChanged.AddListener(HandleChange);
        }

        private void OnDestroy()
        {
            if (_slider != null)
            {
                _slider.onValueChanged.RemoveListener(HandleChange);
            }
        }

        private void HandleChange(float _)
        {
            AudioListener.volume = _slider.value;
        }
    }
}