using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders.Appliers;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(TMP_InputField))]
    public class TextInputDetailBinder : BaseDetailApplier
    {
        [SerializeField, DetailType(typeof(string))]
        private MutableDetailObserver<string> _textDetail;

        private TMP_InputField _inputField;

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            _textDetail.Initialize(_referenceModule.Reference, HandleChange, true);
            HandleChange();
            _inputField.onValueChanged.AddListener(HandleInputChanged);
        }

        private void HandleInputChanged(string inputFieldValue)
        {
            _textDetail.Value = inputFieldValue;
        }

        private void HandleChange()
        {
            if (!_inputField.isFocused)
            {
                _inputField.text = _textDetail.Value;
            }
        }
        
        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _textDetail;
        }
    }
}