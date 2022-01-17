using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.Terms
{
    public class AddTermsChecker : TermsChecker
    {
        [SerializeField]
        private GameObject _button;

        [SerializeField]
        private int _maxSize;
        
        protected override bool DetermineIfReady()
        {
            return (_termList.Value?.Count ?? 0) < _maxSize;
        }

        protected override void ApplyReadyState(bool isReady)
        {
            _button.SetActive(isReady);
        }
    }
}