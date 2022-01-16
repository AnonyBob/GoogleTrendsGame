using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Modules;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    public class ReferenceForwarderBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(IReference))]
        private DetailObserver<IReference> _reference;

        [SerializeField]
        private ReferenceModule[] _targetModules;

        private void Start()
        {
            _reference.Initialize(_referenceModule.Reference, HandleChange, false);
        }

        private void HandleChange()
        {
            if (_reference.Value == null)
            {
                return;
            }
            
            foreach (var target in _targetModules)
            {
                if (target != null)
                {
                    target.Reference = _reference.Value;
                }
            }
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _reference;
        }
    }
}