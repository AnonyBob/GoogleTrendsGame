using System;
using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class IntegerRollUpBinder : BaseDetailBinder
    {
        public bool pauseFill = false;
        
        [SerializeField, DetailType(typeof(int))]
        private DetailObserver<int> _number;

        [SerializeField]
        private bool _resetOnEnable;

        [SerializeField]
        private int _resetValue;
        
        [SerializeField]
        private NumberRollUpLogic _logic;

        private TextMeshProUGUI _text;
        private string _previousValue;
        private bool _wasInitialized;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _number.Initialize(_referenceModule.Reference, HandleChange, false);
            _wasInitialized = true;
        }

        private void OnEnable()
        {
            if (_wasInitialized && _resetOnEnable)
            {
                _logic.SetImmediately(_resetValue);
                _text.text = _logic.StepValue(0);
                
                _logic.SetTarget(_number.Value);
            }
        }

        private void HandleChange()
        {
            _logic.SetTarget(_number.Value);
            _text.text = _logic.StepValue(0);
        }

        private void Update()
        {
            if (!_logic.AtTarget() && !pauseFill)
            {
                var value = _logic.StepValue(Time.deltaTime);
                if (value != _previousValue)
                {
                    _text.text = value;
                    _previousValue = value;
                }
            }
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _number;
        }
    }
}