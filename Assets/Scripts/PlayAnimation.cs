using UnityEngine;

/// <summary>
/// Class to play/toggle a boolean in an animation.
/// </summary>
public class PlayAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string animParam;
    /// <summary>
    /// Toggles the value of an animator parameter (with given name as animParam and animator).
    /// </summary>
    public void ToggleBool()
    {
        Debug.Log("Toggling animation bool (" + animParam + ") for Animator: " + anim);
        bool curr = anim.GetBool(animParam);

        anim.SetBool(animParam, !curr);
    }
}
