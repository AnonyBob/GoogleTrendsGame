using GoogleTrends.GameManagers;
using OwlAndJackalope.UX.Runtime.StateBinders;
using UnityEngine;

namespace GoogleTrends.GameUtility
{
    public class PlaySoundStateBinder : MultiStateBinder<PlaySoundStateBinder.PlaySoundHandler>
    {
        [System.Serializable]
        public class PlaySoundHandler : StateActionHandler
        {
            [SerializeField]
            private AudioClip _sound;

            [SerializeField]
            private bool _oneShot = true;

            protected override void PerformChange(bool currentState)
            {
                if (currentState)
                {
                    SoundManager.PlaySound(_sound, _oneShot);
                }
            }
        }
    }
}