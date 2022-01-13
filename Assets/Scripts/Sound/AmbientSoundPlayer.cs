using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sound
{
    /// <summary>
    /// MonoBehaviour to play random ambient sounds at random intervals
    /// </summary>
    [DisallowMultipleComponent]
    public class AmbientSoundPlayer : MonoBehaviour
    {
        [Tooltip("Minimum delay before sound is played")]
        public float minDelay = 20;
        [Tooltip("Maximum delay before sound is played")]
        public float maxDelay = 50;
        ///<remarks>
        /// If you add a child, it might increase the probability that that object gets played
        /// </remarks>
        [Tooltip("Additional audio sources")]
        public AudioSource[] additionalSources = Array.Empty<AudioSource>();
        private AudioSource[] _sources;
        private bool _stop = true;

        private void Awake()
        {
            List<AudioSource> sources = GetComponentsInChildren<AudioSource>().ToList();
            sources.AddRange(additionalSources);
            _sources = sources.ToArray();
            #if UNITY_EDITOR
            foreach (AudioSource source in _sources)
            {
                if (source.playOnAwake)
                {
                    Debug.LogWarning("Audio source for ambient is play on awake :(");
                }

                if (source.loop)
                {
                    Debug.LogWarning("Audio source for ambient is loop :monkaS:");
                }
            }
#endif
            StartPlaying();
        }

        /// <summary>
        /// Start playing ambient noises, if not already playing
        /// </summary>
        public void StartPlaying()
        {
            if (!_stop) return;
            _stop = false;
            StartCoroutine(nameof(SoundCoroutine));
        }

        /// <summary>
        /// Stop playing ambient noises
        /// </summary>
        public void StopPlaying()
        {
            _stop = true;
            StopCoroutine(nameof(SoundCoroutine));
        }
        
        
        private IEnumerator SoundCoroutine()
        {
            while (!_stop)
            {
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
                int index = Random.Range(0, _sources.Length);
                _sources[index].Play();
            }
        }
    }
}