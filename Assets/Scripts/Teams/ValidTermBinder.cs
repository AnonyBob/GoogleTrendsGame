using System.Collections.Generic;
using GoogleTrends.GameManagers;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Modules;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class ValidTermBinder : BaseDetailBinder
    {
        [SerializeField, DetailType(typeof(string))]
        private DetailObserver<string> _termText;

        [SerializeField, DetailType(typeof(bool))]
        private MutableDetailObserver<bool> _validEntry;

        private ReferenceModule _gameReference;

        private void Start()
        {
            //Ugh, kind of gross.
            _gameReference = GetComponentInParent<RoundManager>().GetComponent<ReferenceModule>();

            _validEntry.Initialize(_referenceModule.Reference);
            _termText.Initialize(_referenceModule.Reference, HandleChange, false);
        }

        private void HandleChange()
        {
            var currentTermText = GetCurrentTermText();
            _validEntry.Value = string.IsNullOrEmpty(_termText.Value) ||
                                _termText.Value.ToLower().Contains(currentTermText.ToLower());
        }

        private string GetCurrentTermText()
        {
            var currentTerm = _gameReference.Reference.GetValue<IReference>(DetailNames.CurrentTerm);
            return currentTerm.GetValue<string>(DetailNames.TermText);
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _termText;
            yield return _validEntry;
        }
    }
}