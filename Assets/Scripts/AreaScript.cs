using System.Linq;
using DefaultNamespace;
using Sound;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Util
{
    /// <summary>
    /// Script attached to a given room/area
    /// </summary>
    /// <remarks>
    /// Any BGM MUST NOT BE MARKED AS PLAY ON AWAKE.
    /// </remarks>
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

        [Tooltip("Whether music should be played on awake.")]
        public bool playMusicOnAwake;

        /// <summary>
        /// Unity Event called when a player passes through a door leaving the area
        /// </summary>
        /// <remarks>
        /// Currently gets called on camera movement phase on door
        /// </remarks>
        [Tooltip("Unity Event called when player passes through a door leaving the area")]
        public UnityEvent onRoomExit = new UnityEvent();
        /// <summary>
        /// Unity Event called when a player passes through a door entering the area
        /// </summary>
        /// <remarks>
        /// Currently gets called on camera movement phase on door after onRoomExit
        /// </remarks>
        [Tooltip("Unity Event called when player passes through a door entering the area")]
        public UnityEvent onRoomEnter = new UnityEvent();
        
        /// <summary>
        /// Currently Playing Background Music
        /// </summary>
        public static AudioSource currentlyActiveBackgroundMusic;
        
        private DoorScript[] _doors;
        private AreaScript[] _adjacentAreas;
        
        private void Awake()
        {
            _doors = transform.GetComponentsInChildren<DoorScript>();
            _adjacentAreas = _doors.Select(door => door.areaToTeleportTo).Where(door => door != null).ToArray();
            if (playMusicOnAwake) PlayBackgroundMusic();
        }

        /// <summary>
        /// Begins to fade out the background music, with a given fade duration.
        /// </summary>
        /// <param name="fadeoutDuration">Time it takes to fade out the music</param>
        public void FadeBackgroundMusic(float fadeoutDuration = 1)
        {
            if (currentlyActiveBackgroundMusic != null && audioMixer != null)
            {
                currentlyActiveBackgroundMusic.outputAudioMixerGroup = musicFadeAudioMixer;
                StartCoroutine(SoundManager.StartMusicFadeOut(currentlyActiveBackgroundMusic, audioMixer,
                    fadeoutDuration, true, true));
                return;
            }
            if (backgroundMusic == null || audioMixer == null) return;
            backgroundMusic.outputAudioMixerGroup = musicFadeAudioMixer;
            StartCoroutine(SoundManager.StartMusicFadeOut(backgroundMusic, audioMixer, fadeoutDuration, true,true));
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
            currentlyActiveBackgroundMusic = backgroundMusic;
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