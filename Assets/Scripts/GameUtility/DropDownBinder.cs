using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropDownBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(string))]
        private MutableDetailObserver<string> _value;

        [SerializeField]
        private TMP_Dropdown _dropDown;

        [SerializeField]
        private DropdownOptions _options;
        
        private void Start()
        {
            _dropDown = GetComponent<TMP_Dropdown>();
            
            _dropDown.options.Clear();
            _dropDown.options.AddRange(_options.GetOptions());
            _value.Initialize(_referenceModule.Reference, HandleChange);
            
            HandleChange();
            _dropDown.onValueChanged.AddListener(HandleInputChanged);
        }

        private void HandleInputChanged(int _)
        {
            _value.Value = _options.GetValueFromIndex(_dropDown.value);
        }

        private void HandleChange()
        {
            _dropDown.value = _options.GetIndex(_value.Value);
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _value;
        }
    }
}