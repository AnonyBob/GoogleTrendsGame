using System.Collections.Generic;
using System.Linq;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.Terms
{
    public abstract class TermsChecker : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(List<IReference>))]
        protected DetailObserver<List<IReference>> _termList;

        protected readonly List<DetailObserver<string>> _termTexts = new List<DetailObserver<string>>();

        protected virtual string DetailName => DetailNames.TermText;
        
        protected virtual void Start()
        {
            _termList.Initialize(_referenceModule.Reference, HandleChange, false);
        }

        private void HandleChange()
        {
            foreach (var termText in _termTexts)
            {
                termText?.Dispose();
            }
            
            _termTexts.Clear();
            foreach (var term in _termList.Value)
            {
                var observer = new DetailObserver<string>() { DetailName = DetailName };
                observer.Initialize(term, CheckIfReady);
                _termTexts.Add(observer);
            }

            CheckIfReady();
        }

        private void CheckIfReady()
        {
            ApplyReadyState(DetermineIfReady());
        }

        protected abstract bool DetermineIfReady();

        protected abstract void ApplyReadyState(bool isReady);

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _termList;
            foreach (var termText in _termTexts)
            {
                yield return termText;
            }
        }
    }
}