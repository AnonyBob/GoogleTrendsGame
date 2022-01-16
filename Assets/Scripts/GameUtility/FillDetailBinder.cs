using System;
using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.GameUtility
{
    public class FillDetailBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(int), typeof(long), typeof(float), typeof(double))]
        private DetailObserver _currentAmount;
        
        [SerializeField, DetailType(typeof(int), typeof(long), typeof(float), typeof(double))]
        private DetailObserver _totalAmount;

        private Image _image;
        
        private void Start()
        {
            _image = GetComponent<Image>();
            _currentAmount.Initialize(_referenceModule.Reference, HandleAmountsChanged);
            _totalAmount.Initialize(_referenceModule.Reference, HandleAmountsChanged);
            
            HandleAmountsChanged();
        }

        private void HandleAmountsChanged()
        {
            if (!_currentAmount.IsSet || !_totalAmount.IsSet)
            {
                _image.fillAmount = 1;
                Debug.LogWarning($"{name} is missing details.");
            }
            
            var total = Convert.ToSingle(_totalAmount.ObjectValue);
            if (total > 0)
            {
                _image.fillAmount = Convert.ToSingle(_currentAmount.ObjectValue) / total;    
            }
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _currentAmount;
            yield return _totalAmount;
        }
    }
}