using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Audio;
using Util;
using static Sound.SoundOutputType;

namespace Sound
{
    /// <summary>
    ///     Represents a sound manager that routes sounds to the correct audio mixer device.
    /// </summary>
    [DisallowMultipleComponent]
    public class SoundManager : MonoBehaviour
    {
        private const float SilentVolume = 0.0001f;
        private const string MusicFadeVolumeParameter = "MusicFadeVolume";
        [SerializeField] private string pathToMasterMixer = "Sound/MasterMixer"; // relative to Resource folder
        private AudioMixer _masterAudioMixer;

        /// <summary>
        ///     Whether this object is initialized (i.e. fields are non null).
        /// </summary>
        /// <returns>Whether the fields are non-null.</returns>
        public bool IsInitialized => _masterAudioMixer != null;
        
        #region Singleton Design Pattern

        private static SoundManager _instance;

        /// <summary>
        ///     Gets a singleton instance of SoundManager.
        /// </summary>
        public static SoundManager Instance
        {
            get
            {
                if (_instance != null) return _instance; // return singleton
                _instance = FindObjectOfType<SoundManager>(); // if null, find Sound manager
                if (_instance == null) // if still null - one does not exist in scene
                {
                    GameObject container = new GameObject("SoundManager"); // create game object
                    _instance = container.AddComponent<SoundManager>(); // add SoundManager
                    _instance.InitializeFields();
                }
                else if (!_instance.IsInitialized)
                {
                    _instance.InitializeFields();
                }

                return _instance; // return singleton
            }
        }

        #endregion

        /// <summary>
        ///     Checks if there's an existing singleton, and if not, initializes fields.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && _instance != this)
                Debug.LogWarning("Multiple DialogueManagers In Scene! Keeping old singleton. " + Instance + ", " +
                                 this);
            if (Instance != null) return;
            InitializeFields();
            _instance = this;
        }

        /// <summary>
        ///     Initializes/locates all fields.
        /// </summary>
        public void InitializeFields()
        {
            _masterAudioMixer = Resources.Load(pathToMasterMixer) as AudioMixer;
            DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Coroutine to fade out the music fade audio mixer group over a set duration.
        /// </summary>
        /// <param name="source">Audio source playing (to fade)</param>
        /// <param name="audioMixer">Audio mixer containing MusicFadeVolume parameter</param>
        /// <param name="duration">Duration of fade</param>
        /// <param name="resetVolume">After stopping the clip, whether the fade should reset so another fade can happen</param>
        /// <returns>Coroutine</returns>
        public static IEnumerator StartMusicFadeOut(IntroAndLoopPlayer source, AudioMixer audioMixer, float duration = 1, bool resetVolume = true, bool shouldClearAreaScriptBGM = false)
        {
            float currentTime = 0;
            audioMixer.GetFloat(MusicFadeVolumeParameter, out float currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, SilentVolume, currentTime / duration);
                audioMixer.SetFloat(MusicFadeVolumeParameter, Mathf.Log10(newVol) * 20);
                yield return null;
            }
            
            source.Stop();
            if (shouldClearAreaScriptBGM)
                AreaScript.currentlyActiveBackgroundMusic = null;
            if (resetVolume)
                audioMixer.SetFloat("MusicFadeVolume", 1);
        }
        
        /// <summary>
        /// Coroutine to fade out the music fade audio mixer group over a set duration.
        /// </summary>
        /// <param name="source">Audio source playing (to fade)</param>
        /// <param name="audioMixer">Audio mixer containing MusicFadeVolume parameter</param>
        /// <param name="duration">Duration of fade</param>
        /// <param name="resetVolume">After stopping the clip, whether the fade should reset so another fade can happen</param>
        /// <returns>Coroutine</returns>
        public static IEnumerator StartMusicFadeOut(AudioSource source, AudioMixer audioMixer, float duration = 1, bool resetVolume = true, bool shouldClearAreaScriptBGM = false)
        {
            float currentTime = 0;
            audioMixer.GetFloat(MusicFadeVolumeParameter, out float currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, SilentVolume, currentTime / duration);
                audioMixer.SetFloat(MusicFadeVolumeParameter, Mathf.Log10(newVol) * 20);
                yield return null;
            }
            
            source.Stop();
            if (shouldClearAreaScriptBGM)
                AreaScript.currentlyActiveBackgroundMusic = null;
            if (resetVolume)
                audioMixer.SetFloat("MusicFadeVolume", 1);
        }

        /// <summary>
        ///     Changes the pitch of selected audio mixer from sound audio type.
        /// </summary>
        /// <param name="type">SoundAudioType of audio mixer.</param>
        /// <param name="newPitch">New pitch of audio mixer.</param>
        public void ChangePitch(SoundOutputType type, float newPitch)
        {
            GetAudioMixerGroup(type).audioMixer.SetFloat(type + " Pitch", newPitch);
        }

        /// <summary>
        ///     Changes the volume of selected audio mixer from sound audio type.
        /// </summary>
        /// <param name="type">SoundAudioType of audio mixer.</param>
        /// <param name="newVolume">New volume of audio mixer, typically in range -80dB to 6dB.</param>
        public void SetVolume(SoundOutputType type, float newVolume)
        {
            GetAudioMixerGroup(type).audioMixer.SetFloat(type + " Volume", newVolume);
        }

        /// <summary>
        ///     Changes the volume of selected audio mixer to lowest possible volume.
        /// </summary>
        /// <param name="type">SoundAudioType of audio mixer.</param>
        /// <remarks>
        ///     This does not keep a copy of the old volume. Any calls to <code>SetVolume(type,float)</code> will unmute it unless
        ///     the newVolume == -80.
        /// </remarks>
        public void Mute(SoundOutputType type)
        {
            SetVolume(type, -80);
        }

        /// <summary>
        ///     Finds the audio mixer group corresponding to the sound output type.
        /// </summary>
        /// <param name="type">SoundOutputType to find the AudioMixerGroup of.</param>
        /// <returns>Corresponding audio mixer group to the SoundOutputType.</returns>
        public AudioMixerGroup GetAudioMixerGroup(SoundOutputType type)
        {
            return _masterAudioMixer.FindMatchingGroups(type.GetAudioMixerString())[0];
        }
    }

    /// <summary>
    ///     Represents a class that holds extension methods for SoundOutputType.
    /// </summary>
    public static class SoundOutputTypeExtensions
    {
        /// <summary>
        ///     Returns correct audio mixer string from given sound output type.
        /// </summary>
        /// <param name="type">SoundOutputType in question.</param>
        /// <returns>AudioMixer identification string..?</returns>
        [SuppressMessage("Style", "IDE0066:Convert switch statement to expression",
            Justification = "Because Unity doesn't support it LOL")]
        public static string GetAudioMixerString(this SoundOutputType type)
        {
            switch (type)
            {
                case Master:
                    return "Master";
                case Music:
                    return "Music";
                case Dialogue:
                    return "Dialogue";
                default:
                    return "SFX";
            }
        }
    }

    /// <summary>
    ///     Represents the type of sound playing.
    /// </summary>
    public enum SoundOutputType
    {
        Master,
        Music,
        Dialogue,
        SFX,
        Ambient
    }
}