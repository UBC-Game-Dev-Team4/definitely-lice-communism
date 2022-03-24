using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Sound
{
    /// <summary>
    /// MonoBehaviour to play random ambient sounds at random intervals.
    /// The sounds selected to play are children of this object and additional sources in the list.
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
        [Tooltip("Additional audio sources (children are automatically added)")]
        public AudioSource[] additionalSources = Array.Empty<AudioSource>();
        [Tooltip("Whether the length of the audio clip is added to the delay")]
        public bool accountForAudioLength;
        [Tooltip("Whether the music starts playing on awake")]
        public bool playOnAwake = true;
        [Tooltip("Whether there is an initial delay")]
        public bool delayOnStart = true;
        [Tooltip("Whether the sounds fade on stop")]
        public bool fadeOnStop;
        [Tooltip("Audio mixer")]
        public AudioMixer musicMixer;
        [Tooltip("Audio mixer group")]
        public AudioMixerGroup musicAudioGroup;
        [Tooltip("Audio mixer group to be switched to on fade")]
        public AudioMixerGroup musicFadeGroup;
        private AudioSource[] _sources;
        private int _currentIndex = -1;
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
                    Debug.LogWarning("Audio source for ambient is loop: problems may arise");
                }
            }
#endif
            if (playOnAwake) StartPlaying();
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
            if (_currentIndex >= 0 && _currentIndex < _sources.Length)
            {
                if (fadeOnStop)
                {
                    _sources[_currentIndex].outputAudioMixerGroup = musicFadeGroup;
                    StartCoroutine(SoundManager.StartMusicFadeOut(_sources[_currentIndex], musicMixer));
                }
                else _sources[_currentIndex]?.Stop();
            }
            _currentIndex = -1;
        }
        
        
        private IEnumerator SoundCoroutine()
        {
            if (delayOnStart)
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            while (!_stop)
            {
                int index = Random.Range(0, _sources.Length);
                _currentIndex = index;
                if (fadeOnStop)
                    _sources[_currentIndex].outputAudioMixerGroup = musicAudioGroup;
                _sources[index].Play();
                if (accountForAudioLength)
                {
                    yield return new WaitForSeconds(_sources[index].clip.length);
                    _currentIndex = -1;
                }
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            }
        }
    }
}