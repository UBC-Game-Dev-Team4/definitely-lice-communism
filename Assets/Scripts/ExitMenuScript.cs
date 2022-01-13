using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Interactable script to create the stage complete menu
    /// </summary>
    public class ExitMenuScript : Interactable
    {
        [Tooltip("GameObject of UI to enable")]
        public GameObject menuToEnable;
        
        /// <inheritdoc cref="Interactable" />
        public override void Interact(object src, params object[] args)
        {
            base.Interact(src, args);
            if (menuToEnable != null)
            {
                menuToEnable.SetActive(true);
            }
        }
    }
}