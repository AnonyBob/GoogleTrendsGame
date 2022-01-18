using GoogleTrends.GameManagers;
using UnityEngine;

namespace GoogleTrends.UI
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _sound;

        [SerializeField]
        private AudioClip[] _sounds;
        
        [SerializeField]
        private bool _oneShot = true;

        private AudioSource _source;

        public void PlaySound()
        {
            _source = SoundManager.PlaySound(_sound, _oneShot);
        }

        public void StopSound()
        {
            if (_source != null)
            {
                _source.Stop();
            }
        }

        public void PlaySound(int index)
        {
            if (index >= 0 && index < _sounds.Length)
            {
                SoundManager.PlaySound(_sounds[index], _oneShot);
            }
        }
    }
}