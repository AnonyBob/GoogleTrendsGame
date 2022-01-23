using System.Collections.Generic;
using GoogleTrends.GameUtility.Selection;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.Modules;
using UnityEngine;
using UnityEngine.Serialization;

namespace GoogleTrends.Terms
{
    [RequireComponent(typeof(ReferenceModule))]
    public class SettingsReferenceForwarder : ReferenceProvider
    {
        [FormerlySerializedAs("_targetModule")] [SerializeField]
        private ReferenceModule _sourceModule;

        [SerializeField]
        private SelectionGroup _selectionGroup;

        private ReferenceModule _targetModule;
        private bool _initialized = false;
        
        private void Start()
        {
            _sourceModule.Reference.VersionChanged += HandleChange;
            _targetModule = GetComponent<ReferenceModule>();
            _initialized = true;
            
            HandleChange();
        }

        private void OnDestroy()
        {
            if (_sourceModule != null)
            {
                _sourceModule.Reference.VersionChanged -= HandleChange;    
            }
        }

        private void HandleChange()
        {
            if (_targetModule != null)
            {
                _targetModule.AddDetails(ProvideReference());    
            }
        }

        public void SelectRound()
        {
            var selection = _selectionGroup.FirstSelection;
            if (selection != null)
            {
                _targetModule.AddDetails(selection);
                _targetModule.Reference.GetMutable<bool>(DetailNames.ShowCurrentTerm).SetValue(true);
            }
            else
            {
                _targetModule.Reference.GetMutable<bool>(DetailNames.ShowCurrentTerm).SetValue(false);
            }
        }
        
        public override IEnumerable<IDetail> ProvideReference()
        {
            if (_initialized)
            {
                yield return _sourceModule.Reference.GetDetail<int>(DetailNames.TimerMax);
                yield return _sourceModule.Reference.GetDetail<string>(DetailNames.DateRange);
                yield return _sourceModule.Reference.GetDetail<string>(DetailNames.GeoLocation);
                yield return _sourceModule.Reference.GetDetail<bool>(DetailNames.SetTeamNamesOnFirstRound);
                yield return _sourceModule.Reference.GetDetail<bool>(DetailNames.FirstRoundWorthPoints);
            }
        }
    }
}