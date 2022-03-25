using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Script attached to the deep fryer to handle killing logic
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class DeepFryerScript : Interactable
    {
        private ChefScript _chef = null;
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
            }
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