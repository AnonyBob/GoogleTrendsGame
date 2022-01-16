using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.DetailBinders.Appliers;
using OwlAndJackalope.UX.Runtime.Modules;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.GameUtility.Selection
{
    public class SelectionObject : BaseDetailApplier
    {
        private SelectionGroup _group;
        private IReference _currentReference;

        [SerializeField, DetailType(typeof(bool))]
        private DetailObserver<bool> _selectedDetail;

        private void Start()
        {
            _group = GetComponentInParent<SelectionGroup>();
            _currentReference = _referenceModule.Reference;
            _selectedDetail.Initialize(_currentReference, null, false);

            if (_group != null)
            {
                _group.Register(_currentReference);    
            }
            else
            {
                Debug.LogWarning($"{name} does not have a selection group in parent");
            }
        }

        public void Select()
        {
            if (_group != null)
            {
                _group.Select(_currentReference);
            }
        }

        public void Deselect()
        {
            if (_group != null)
            {
                _group.Deselect(_currentReference);
            }
        }

        public void ToggleSelect()
        {
            if (_selectedDetail.Value)
            {
                Deselect();
            }
            else
            {
                Select();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_group != null)
            {
                _group.Unregister(_currentReference);
            }
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _selectedDetail;
        }
    }
}