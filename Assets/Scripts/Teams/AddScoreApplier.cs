using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders.Appliers;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;

namespace GoogleTrends.Teams
{
    public class AddScoreApplier : BaseDetailApplier
    {
        [SerializeField, DetailType(typeof(int))]
        private MutableDetailObserver<int> _score;

        private void Start()
        {
            _score.Initialize(_referenceModule.Reference);
        }

        public void ChangeScore(int amount)
        {
            _score.Value += amount;
        }

        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _score;
        }
    }
}