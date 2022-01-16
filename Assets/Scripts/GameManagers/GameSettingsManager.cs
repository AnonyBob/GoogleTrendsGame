using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.Data;
using OwlAndJackalope.UX.Runtime.Data.Extensions;
using OwlAndJackalope.UX.Runtime.DetailBinders;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.GameManagers
{
    public class GameSettingsManager : BaseDetailBinder
    {
        private IMutableCollectionDetail<IReference> Terms =>
            _referenceModule.Reference.GetMutableCollection<IReference>(DetailNames.Terms);
        
        public void AddTerm()
        {
            Terms.Add(new BaseReference(
                new BaseDetail<string>(DetailNames.TermText),
                new BaseDetail<string>(DetailNames.BonusTerm),
                new BaseDetail<int>(DetailNames.BonusTermPoints),
                new BaseDetail<int>(DetailNames.Multiplier)));
        }

        public void MoveTerm(int oldIndex, int newIndex)
        {
            var terms = Terms;
            var newValue = terms[newIndex];
            terms[newIndex] = terms[oldIndex];
            terms[oldIndex] = newValue;
        }
        
        public void RemoveTerm(int index)
        {
            Terms.RemoveAt(index);
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield break;
        }
    }
}