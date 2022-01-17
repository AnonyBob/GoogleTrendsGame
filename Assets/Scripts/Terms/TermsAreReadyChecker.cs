using System.Collections.Generic;
using System.Linq;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;
using UnityEngine.UI;

namespace GoogleTrends.Terms
{
    public class TermsAreReadyChecker : TermsChecker
    {
        [SerializeField]
        private Button _button;

        protected override bool DetermineIfReady()
        {
            var count = _termTexts.Select(t => t.Value)
                .Distinct()
                .Count(t => !string.IsNullOrEmpty(t));

            return  count == _termTexts.Count && _termTexts.Count > 0;
        }

        protected override void ApplyReadyState(bool isReady)
        {
            _button.interactable = isReady;
        }

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