using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Audio;
using static Sound.SoundOutputType;

namespace Sound
{
    /// <summary>
    ///     Represents a sound manager that routes sounds to the correct audio mixer device.
    ///     TODO: use to extend Singleton<SoundManager></SoundManager>
    /// </summary>
    [DisallowMultipleComponent]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private string pathToMasterMixer = "Sound/MasterMixer"; // relative to Resource folder
        private AudioMixer _masterAudioMixer;

        /// <summary>
        ///     Whether this object is initialized (i.e. fields are non null).
        /// </summary>
        /// <returns>Whether the fields are non-null.</returns>
        public bool IsInitialized => _masterAudioMixer != null;

        /// <summary>
        ///     Checks if there's an existing singleton, and if not, initializes fields.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && _instance != this)
                Debug.LogWarning("Multiple DialogueManagers In Scene! Keeping old singleton. " + Instance + ", " +
                                 this);
            if (Instance == null)
            {
                InitializeFields();
                _instance = this;
            }
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
        private AudioMixerGroup GetAudioMixerGroup(SoundOutputType type)
        {
            return _masterAudioMixer.FindMatchingGroups(type.GetAudioMixerString())[0];
        }

        /// <summary>
        ///     Plays a sound to a given sound output mixer.
        ///     Note that you can instead, in inspector, change the sound output audio mixer of the audio source, as this does the
        ///     same thing.
        /// </summary>
        /// <param name="source">AudioSource in question.</param>
        /// <param name="type">SoundOutputType of source (e.g. master).</param>
        public void PlaySound(AudioSource source, SoundOutputType type)
        {
            source.outputAudioMixerGroup = GetAudioMixerGroup(type);
            source.Play();
        }

        /// <summary>
        ///     Stops a sound currently playing.
        ///     I don't know why you would call this.
        /// </summary>
        /// <param name="source">AudioSource in question.</param>
        public void StopSound(AudioSource source)
        {
            source.Stop();
        }

        #region Singleton Design Pattern

        private static SoundManager _instance;

        /// <summary>
        ///     Gets a singleton instance of SoundManager.
        /// </summary>
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SoundManager>(); // if null, find Sound manager
                    if (_instance == null) // if still null - one does not exist in scene
                    {
                        GameObject container = new GameObject("SoundManager"); // create gameobject
                        _instance = container.AddComponent<SoundManager>(); // add Soundmanager
                        _instance.InitializeFields();
                    }
                    else if (!_instance.IsInitialized)
                    {
                        _instance.InitializeFields();
                    }
                }

                return _instance; // return singleton
            }
        }

        #endregion
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