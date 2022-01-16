using System;
using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.GameUtility
{
    [RequireComponent(typeof(Image))]
    public class BarSizeRollUpBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(int))]
        private DetailObserver<int> _number;

        [SerializeField]
        private float _maxNumberSize = 100;
        
        [SerializeField]
        private float _maxBarSize = 100;

        [SerializeField]
        private bool _useHorizontal = true;
        
        [SerializeField]
        private bool _resetOnEnable;

        [SerializeField]
        private int _resetValue;
        
        [SerializeField]
        private NumberRollUpLogic _logic;

        private Image _image;
        private string _previousValue;
        private bool _wasInitialized;

        private void Start()
        {
            _image = GetComponent<Image>();
            _number.Initialize(_referenceModule.Reference, HandleChange, false);
            _wasInitialized = true;
        }

        private void OnEnable()
        {
            if (_wasInitialized && _resetOnEnable)
            {
                _logic.SetImmediately(_resetValue);
                SetSize(_logic.StepValueAsNumber(0));
                
                _logic.SetTarget(_number.Value);
            }
        }

        private void HandleChange()
        {
            _logic.SetTarget(_number.Value);
            SetSize(_logic.StepValueAsNumber(0));
        }

        private void Update()
        {
            if (!_logic.AtTarget())
            {
                SetSize(_logic.StepValueAsNumber(Time.deltaTime));
            }
        }

        private void SetSize(float currentNumber)
        {
            var ratio = currentNumber / _maxNumberSize;
            var finalSize = ratio * _maxBarSize;
            
            var size = _image.rectTransform.sizeDelta;
            if (_useHorizontal)
            {
                size.x = finalSize;
            }
            else
            {
                size.y = finalSize;
            }

            _image.rectTransform.sizeDelta = size;
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _number;
        }
    }
}