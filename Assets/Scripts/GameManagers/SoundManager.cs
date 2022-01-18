using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoogleTrends.GameManagers
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        [SerializeField]
        private AudioSource _oneshotSource;

        [SerializeField]
        private AudioSource _template;
        
        [SerializeField]
        private List<AudioSource> _sourcePool = new List<AudioSource>();
        
        private readonly List<AudioSource> _playingSources = new List<AudioSource>();

        public static AudioSource PlaySound(AudioClip sound, bool oneShot = true)
        {
            if (sound == null)
            {
                return null;
            }
            
            if (oneShot)
            {
                Instance._oneshotSource.PlayOneShot(sound);
                return null;
            }
            
            return Instance.PlaySoundInstance(sound);
        }

        private AudioSource PlaySoundInstance(AudioClip sound)
        {
            ReturnPlayingSources();
            if (_sourcePool.Count == 0)
            {
                _sourcePool.Add(CreateNewAudioSource());
            }
            
            var source = _sourcePool[0];
            _sourcePool.RemoveAt(0);

            source.clip = sound;
            source.Play();
                
            _playingSources.Add(source);
            return source;
        }

        private AudioSource CreateNewAudioSource()
        {
            var source = Instantiate(_template, transform);
            return source;
        }

        private void ReturnPlayingSources()
        {
            for (var i = 0; i < _playingSources.Count; ++i)
            {
                if (!_playingSources[i].isPlaying)
                {
                    _sourcePool.Add(_playingSources[i]);
                    _playingSources.RemoveAt(i);

                    i--;
                }
            }
        }
    }
}