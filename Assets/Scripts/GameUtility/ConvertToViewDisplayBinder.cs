using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    public class ConvertToViewDisplayBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(string))]
        private DetailObserver<string> _value;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private DropdownOptions _options;

        private void Start()
        {
            _value.Initialize(_referenceModule.Reference, HandleChange, false);
        }

        private void HandleChange()
        {
            var optionIndex = _options.GetIndex(_value.Value);
            if (optionIndex > 0)
            {
                _text.text = _options.GetDisplayValueFromIndex(optionIndex);
            }
            else
            {
                _text.text = string.Empty;
            }
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _value;
        }
    }
}