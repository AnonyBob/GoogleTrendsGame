using UnityEngine;

namespace GoogleTrends.GameUtility
{
    [System.Serializable]
    public class NumberRollUpLogic
    {
        [SerializeField, Tooltip("Time it takes for the number to change to target.")]
        private float _duration = 1f;

        [SerializeField, Tooltip("The max difference that the two numbers can have. When greater " +
                                 "than zero this will scale the duration so smaller differences take less time.")]
        private float _maxDifference = 100f;

        [SerializeField, Tooltip("The format string that the number will be contained in.")]
        private string _formatString = "{0}";

        [SerializeField, Tooltip("The formatting string to use for ensuring the look of the number.")]
        private string _numberFormatString = "N0";

        [SerializeField, Tooltip("When true this will set the initial target immediately.")]
        private bool _setInitialToTarget;

        [SerializeField]
        private AnimationCurve _easing;

        private bool _hasInitialized;
        private float _currentNumber;
        private float _startingNumber;
        
        private float _currentTime = 0;
        private float _currentDuration = 0;
        private float _targetNumber;
        
        public void SetTarget(float targetNumber)
        {
            if (!_hasInitialized && _setInitialToTarget)
            {
                SetImmediately(targetNumber);
                return;
            }
            _targetNumber = targetNumber;
            _startingNumber = _currentNumber;
            _currentTime = 0;
            _currentDuration = CalculateDuration();
        }

        public string SetImmediately(float targetNumber)
        {
            SetImmediatelyAsNumber(targetNumber);
            return FormatNumber();
        }

        public float SetImmediatelyAsNumber(float targetNumber)
        {
            _hasInitialized = true;
            _currentNumber = targetNumber;
            _currentTime = 0;
            _currentDuration = 0;

            return _currentNumber;
        }

        public bool AtTarget()
        {
            return Mathf.Approximately(_currentNumber, _targetNumber);
        }

        public string StepValue(float deltaTime)
        {
            StepValueAsNumber(deltaTime);
            return FormatNumber();
        }

        public float StepValueAsNumber(float deltaTime)
        {
            if (_currentDuration <= 0)
            {
                return _currentNumber;
            }
            
            _currentTime += deltaTime;
            _currentNumber = Mathf.Lerp(_startingNumber, _targetNumber,
                _easing.Evaluate(_currentTime / _currentDuration));
            return _currentNumber;
        }

        private float CalculateDuration()
        {
            var diff = Mathf.Abs(_targetNumber - _currentNumber);
            if (_maxDifference <= 0 || diff > _maxDifference)
            {
                return _duration;
            }

            return _duration * (diff / _maxDifference);
        }
        
        private string FormatNumber()
        {
            return string.Format(_formatString, _currentNumber.ToString(_numberFormatString));
        }
    }
}