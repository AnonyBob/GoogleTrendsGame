using OwlAndJackalope.UX.Runtime.StateBinders;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GoogleTrends.GameUtility
{
    public class AnimationStateBinder : MultiStateBinder<AnimationStateBinder.AnimationStateHandler>
    {
        [System.Serializable]
        public class AnimationStateHandler : StateActionHandler
        {
            [SerializeField]
            private string _animationName;
            private Animation _animator;

            protected override void InitializeGameObject()
            {
                _animator = _gameObject.GetComponent<Animation>();
            }

            protected override void PerformChange(bool isActive)
            {
                if (isActive)
                {
                    _animator.Stop();
                    _animator.Play(_animationName);
                }
            }
        }
    }
}