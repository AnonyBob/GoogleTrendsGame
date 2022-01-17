using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NumberInputDetailBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(int))]
        private MutableDetailObserver<int> _numberDetail;

        private TMP_InputField _inputField;

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _numberDetail.Initialize(_referenceModule.Reference, HandleChange, true);
            HandleChange();
            _inputField.onValueChanged.AddListener(HandleInputChanged);
        }

        private void HandleInputChanged(string inputFieldValue)
        {
            if (int.TryParse(inputFieldValue, out var value))
            {
                _numberDetail.Value = value;    
            }
            
        }

        private void HandleChange()
        {
            if (!_inputField.isFocused)
            {
                _inputField.text = _numberDetail.Value.ToString("N0");
            }
        }
        
        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _numberDetail;
        }
    }
}