using DefaultNamespace;
using UnityEngine;
using Util;

namespace LevelOne
{
    /// <summary>
    /// Script attached to the deep fryer to handle killing logic
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class DeepFryerScript : Interactable
    {
        [Tooltip("Displayed oil object")]
        public GameObject displayedOil;

        [Tooltip("Audio that normally plays")] public AudioSource normalAudio;
        [Tooltip("Audio to switch to on kill")]
        public AudioSource onKillAudio;
        [Tooltip("Audio on dunk")]
        public AudioSource dunkNoise;
        private ChefScript _chef = null;
        private bool _killed = false;
        private AreaScript _area;
        
        private void Awake()
        {
            _area = GetComponentInParent<AreaScript>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ChefScript chef = other.gameObject.GetComponent<ChefScript>();
            if (chef != null)
            {
                _chef = chef;
            }
        }

        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (_chef != null)
            {
                _chef.Kill(true);
                dunkNoise.Play();
                OnKill();
            }
        }

        /// <summary>
        /// Plays the audio
        /// </summary>
        public void PlayAudio()
        {
            if (_killed) onKillAudio.Play();
            else normalAudio.Play();
        }

        /// <summary>
        /// Stops the audio
        /// </summary>
        public void StopAudio()
        {
            onKillAudio.Stop();
            normalAudio.Stop();
        }

        /// <summary>
        /// Runs on kill - makes oil active and plays the kill audio
        /// </summary>
        public void OnKill()
        {
            displayedOil.SetActive(true);
            _killed = true;
            StopAudio();
            if (AreaScript.currentArea == _area)
                onKillAudio.Play();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            ChefScript chef = other.gameObject.GetComponent<ChefScript>();
            if (chef != null)
            {
                _chef = null;
            }
        }
    }
}