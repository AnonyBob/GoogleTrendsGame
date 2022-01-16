using System.Collections;
using System.Collections.Generic;
using OwlAndJackalope.UX.Runtime.DetailBinders.Appliers;
using OwlAndJackalope.UX.Runtime.Observers;
using UnityEngine;
using UnityEngine.Events;

namespace GoogleTrends.GameUtility
{
    public class TimerDetailApplier : BaseDetailApplier
    {
        [SerializeField, DetailType(typeof(int))]
        private MutableDetailObserver<int> _time;

        [SerializeField]
        private bool _startOnAwake;
        
        [SerializeField]
        private UnityEvent _onTimerComplete;
        
        private Coroutine _routine;
        
        private void Start()
        {
            _time.Initialize(_referenceModule.Reference);
            if (_startOnAwake && _time.IsSet)
            {
                StartTimer();
            }
        }

        public void StartTimer()
        {
            StopTimer();
            _routine = StartCoroutine(Tick());
        }

        public void StopTimer()
        {
            if (_routine != null)
            {
                StopCoroutine(_routine);    
            }
        }

        private IEnumerator Tick()
        {
            while (_time.IsSet && _time.Value > 0)
            {
                yield return new WaitForSeconds(1);
                if (_time.IsSet)
                {
                    _time.Value--;    
                }
            }
            
            _onTimerComplete?.Invoke();
        }
        
        protected override IEnumerable<AbstractDetailObserver> GetDetailObservers()
        {
            yield return _time;
        }
    }
}