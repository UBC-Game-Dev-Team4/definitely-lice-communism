using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
    /// <summary>
    /// Class to first play an intro and loop another audio source
    /// </summary>
    public class IntroAndLoopPlayer : MonoBehaviour
    {
        [Tooltip("Audio source to play as an intro")]
        public AudioSource intro;
        [Tooltip("Audio source to loop")]
        public AudioSource loop;

        /// <summary>
        /// Current active audio source
        /// </summary>
        public AudioSource ActiveAudioSource => intro.isPlaying ? intro : loop;

        /// <summary>
        /// Sets the active audio mixer group
        /// </summary>
        public AudioMixerGroup MixerGroup
        {
            get => intro.outputAudioMixerGroup;
            set
            {
                intro.outputAudioMixerGroup = value;
                loop.outputAudioMixerGroup = value;
            }
        }
        
        private void Awake()
        {
            if (intro.playOnAwake)
                loop.PlayDelayed(intro.clip.length);
            MixerGroup = MixerGroup;
        }

        /// <summary>
        /// Play intro + queue loop
        /// </summary>
        public void Play()
        {
            intro.Play();
            loop.PlayDelayed(intro.clip.length);
        }

        /// <summary>
        /// Stop intro or loop
        /// </summary>
        public void Stop()
        {
            intro.Stop();
            loop.Stop();
        }

        /// <summary>
        /// If loop hasn't been playing, remove it from queue
        /// </summary>
        public void StopQueue()
        {
            if (intro.isPlaying) loop.Stop();
        }

        /// <summary>
        /// Determines if the clip of intro/loop is the same as the other intro/loop
        /// </summary>
        /// <param name="other">Other IntroAndLoopPlayer</param>
        /// <returns>Whether sound clips are equal</returns>
        public bool SameSound(IntroAndLoopPlayer other)
        {
            return intro.clip == other.intro.clip && loop.clip == other.loop.clip;
        }
    }
}