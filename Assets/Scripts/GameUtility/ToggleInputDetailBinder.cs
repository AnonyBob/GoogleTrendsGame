using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleInputDetailBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(bool))]
        private MutableDetailObserver<bool> _boolDetail;

        private Toggle _inputField;

        private void Start()
        {
            _inputField = GetComponent<Toggle>();
            _boolDetail.Initialize(_referenceModule.Reference, HandleChange, true);
            HandleChange();
            _inputField.onValueChanged.AddListener(HandleInputChanged);
        }

        private void HandleInputChanged(bool inputFieldValue)
        {
            _boolDetail.Value = inputFieldValue;
        }

        private void HandleChange()
        {
            _inputField.isOn = _boolDetail.Value;
        }
        
        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _boolDetail;
        }
    }
}