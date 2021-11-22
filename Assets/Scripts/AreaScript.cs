using System.Linq;
using DefaultNamespace;
using Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace Util
{
    /// <summary>
    /// Script attached to a given room/area
    /// </summary>
    public class AreaScript : MonoBehaviour
    {
        [Tooltip("Player's camera state on enter")]
        public CameraState cameraStateOnEnter = CameraState.CreateFreeXYCameraState(new Vector3(0, 0, -10));

        [Tooltip("Background music to play upon enter?")]
        public AudioSource backgroundMusic;

        [Tooltip("Background Music Target Audio Mixer")]
        public AudioMixerGroup musicTargetAudioMixer;

        [Tooltip("Background music fade audio mixer")]
        public AudioMixerGroup musicFadeAudioMixer;

        [Tooltip("Background Music audio mixer")]
        public AudioMixer audioMixer;
        
        private DoorScript[] _doors;
        private AreaScript[] _adjacentAreas;
        
        private void Awake()
        {
            _doors = transform.GetComponentsInChildren<DoorScript>();
            _adjacentAreas = _doors.Select(door => door.areaToTeleportTo).Where(door => door != null).ToArray();
        }

        /// <summary>
        /// Begins to fade out the background music, with a given fade duration.
        /// </summary>
        /// <param name="fadeoutDuration">Time it takes to fade out the music</param>
        public void FadeBackgroundMusic(float fadeoutDuration = 1)
        {
            if (backgroundMusic == null || audioMixer == null) return;
            backgroundMusic.outputAudioMixerGroup = musicFadeAudioMixer;
            StartCoroutine(SoundManager.StartMusicFadeOut(backgroundMusic, audioMixer, fadeoutDuration, true));
        }

        
        /// <summary>
        /// Determines whether this area has its own background music.
        /// </summary>
        /// <returns>Boolean of whether background music is defined</returns>
        public bool HasBackgroundMusic() => backgroundMusic != null;

        /// <summary>
        /// Starts playing background music, if it exists
        /// </summary>
        public void PlayBackgroundMusic()
        {
            if (backgroundMusic == null) return;
            if (musicTargetAudioMixer != null)
                backgroundMusic.outputAudioMixerGroup = musicTargetAudioMixer;
            backgroundMusic.Play();
        }

        /// <summary>
        /// On editor value change, log an error if the music fade audio mixer is not defined
        /// </summary>
        public void OnValidate()
        {
            if (musicFadeAudioMixer == null)
                Debug.LogError("Music Fade Audio Mixer should not be null.");
        }
    }
}